using InvoiceGenerator2.Core.Extensions;
using InvoiceGenerator2.Helpers;
using InvoiceGenerator2.Models;
using InvoiceGenerator2.Models.ViewModels.Invoices;
using InvoiceGenerator2.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace InvoiceGenerator2.Core.Controllers
{
    [Authorize(Roles = "" + RoleName.Admin + ", " + RoleName.Invoicing + "")]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TDCDbContext _tdcContext;
        private readonly ApplicationConfiguration _applicationConfiguration;
        private readonly WaybillNumberService _waybillNumberService;
        private readonly IWebHostEnvironment _environment;

        public InvoicesController(ApplicationDbContext context, TDCDbContext tdcDbContext, ApplicationConfiguration applicationConfiguration,
            WaybillNumberService waybillNumberService, IWebHostEnvironment environment)
        {
            _context = context;
            _tdcContext = tdcDbContext;
            _applicationConfiguration = applicationConfiguration;
            _waybillNumberService = waybillNumberService;
            _environment = environment;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Invoices
        public ActionResult Index()
        {
            List<IndexInvoiceViewModel> invoices = null;

            DateTime monthsAgo = DateTime.Now.AddMonths(-3);

            IQueryable<Invoice> query = _context.Invoices.Include(c => c.Client).Where(i => i.InvoiceDate >= monthsAgo);


            if (!User.IsInRole(RoleName.Admin))
            {
                query = query.Where(i => i.IsDeleted == false);
            }

            if (User.IsInRole(RoleName.ThirdParty))
            {
                string userId = User.GetUserId();
                ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id.Equals(userId));
                query = query.Where(i => i.BranchCode == user.BranchCode);
            }

            List<Branch> branches = _context.Branches.ToList();

            invoices = query
                .OrderByDescending(i => i.InvoiceDate)
                .ToList()
                .Select(x => new IndexInvoiceViewModel
                {
                    Id = x.Id,
                    Client = x.Client,
                    InvoiceDate = x.InvoiceDate,
                    PoNumber = x.PoNumber,
                    GeneralWaybillNumber = x.GeneralWaybillNumber,
                    BranchCode = x.BranchCode,
                    BranchName = branches.FirstOrDefault(b => b.Code.Equals(x.BranchCode))?.Name,
                    Status = x.Status,
                    CreatedDate = x.CreatedDate,
                    CreatedUser = x.CreatedUser,
                    ModifiedDate = x.ModifiedDate,
                    ModifiedUser = x.ModifiedUser,
                    DeletedDate = x.DeletedDate,
                    DeletedUser = x.DeletedUser,
                    IsDeleted = x.IsDeleted
                }).ToList();

            foreach (IndexInvoiceViewModel t in invoices)
            {
                string waybillNumber = "";

                if (t.GeneralWaybillNumber != null)
                {
                    waybillNumber = t.BranchCode;
                }

                t.GeneralWaybillNumber += waybillNumber;
            }

            return View(invoices);
        }

        public ViewResult Create()
        {
            string userId = User.GetUserId() ?? string.Empty;
            Users user = _tdcContext.Users.FirstOrDefault(x => x.Id == userId && x.IsDeleted == false);
            IQueryable<Product> productQuery = _context.Products.Include("Branches").Where(p => p.IsDeleted == false);
            IQueryable<Branch> branchQuery = _context.Branches.Where(b => b.IsDeleted == false);
            IQueryable<Client> clientQuery = _context.Clients.Include("Branches").Where(b => b.IsDeleted == false);
            Branch branch = _context.Branches.FirstOrDefault(b => b.Code == user.BranchCode);
            if (User.IsInRole(RoleName.ThirdParty))
            {
                branchQuery = branchQuery.Where(b => b.Code.Equals(user.BranchCode));
                productQuery = productQuery.Where(p => p.Branches.Count(b => b.BranchId == branch.Id) > 0);
                clientQuery = clientQuery.Where(p => p.Branches.Count(b => b.BranchId == branch.Id) > 0);
            }

            List<Branch> branchesList = branchQuery.OrderBy(n => n.Name).ToList();
            List<Client> clientsList = clientQuery.OrderBy(d => d.Name).ToList();
            List<Product> productsList = productQuery.ToList();


            List<Transporter> transportersList = _context.Transporters.Where(b => b.IsDeleted == false).OrderBy(n => n.Name).ToList();

            CreateInvoiceViewModel viewModel = new CreateInvoiceViewModel
            {
                Branches = branchesList,
                Clients = clientsList,
                Products = productsList,
                Transporters = transportersList,
                BranchCode = user.BranchCode,
                Status = "PD"
            };

            return View(viewModel);
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=347598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateInvoiceViewModel invoice)
        {
            if (!ModelState.IsValid) return View();
            DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

            string newGuid = Guid.NewGuid().ToString();
            string createdUser = User.Identity.Name;
            Invoice newInvoice = new Invoice();

            newInvoice.Id = newGuid;
            newInvoice.ClientId = invoice.ClientId;
            newInvoice.InvoiceDate = invoice.InvoiceDate;
            newInvoice.PoNumber = invoice.PoNumber;
            newInvoice.BranchCode = invoice.BranchCode;
            newInvoice.Status = invoice.Status;
            newInvoice.TransporterPoNumber = invoice.TransporterPoNumber;

            if (!string.IsNullOrEmpty(invoice.TransporterId))
            {
                newInvoice.TransporterId = invoice.TransporterId;
            }

            if (invoice.Status == "PD")
            {
                newInvoice.GeneralWaybillNumber = null;
            }

            Setting settings = _context.Settings.SingleOrDefault(x => x.Id == _applicationConfiguration.ConfigKey);

            if (invoice.Status == "DL")
            {
                if (settings != null)
                {
                    int waybillNumber = settings.LastWaybillNumber + 1;
                    newInvoice.GeneralWaybillNumber = waybillNumber.ToString();
                }
                else
                {
                    newInvoice.GeneralWaybillNumber = null;
                }
            }

            newInvoice.CreatedDate = dateTime;
            newInvoice.CreatedUser = createdUser;

            for (int i = 0; i < invoice.InvoiceItems.Count; i++)
            {
                InvoiceItem newInvoiceItem = new InvoiceItem();

                newInvoiceItem.InvoiceId = newGuid;
                newInvoiceItem.ProductId = invoice.InvoiceItems[i].ProductId;
                newInvoiceItem.Quantity = invoice.InvoiceItems[i].Quantity;
                newInvoiceItem.UnitSize = invoice.InvoiceItems[i].UnitSize;
                newInvoiceItem.TotalKg = invoice.InvoiceItems[i].TotalKg;
                newInvoiceItem.Pallets = invoice.InvoiceItems[i].Pallets;
                newInvoiceItem.BatchNumbers = invoice.InvoiceItems[i].BatchNumbers;
                newInvoiceItem.CreatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");
                ;
                newInvoiceItem.CreatedUser = createdUser;
                newInvoiceItem.Order = i;

                _context.InvoiceItems.Add(newInvoiceItem);
            }

            _context.Invoices.Add(newInvoice);
            _context.SaveChanges();

            if (invoice.Status == "DL")
            {
                if (settings != null)
                {
                    // Update Last Waybill Number
                    _waybillNumberService.UpdateWaybillNumber(settings.LastWaybillNumber + 1, createdUser);
                }
            }

            if (invoice.SaveAndDownload)
            {
                return Redirect($"/Invoices/Details/{newGuid}?download=true");
            }

            return RedirectToAction("Details", new { Id = newGuid });
        }


        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }


            string shouldDownload = Request.Query["download"];


            Invoice? invoice = null;
            if (User.IsInRole(RoleName.Admin))
            {
                invoice = _context.Invoices
                    .Include(c => c.Client)
                    .Include(t => t.Transporter)
                    .Include(i => i.InvoiceItems.Select(it => it.Product))
                    .SingleOrDefault(x => x.Id == id);
            }
            else
            {
                invoice = _context.Invoices.Include(c => c.Client).Include(t => t.Transporter).Include(i => i.InvoiceItems.Select(it => it.Product))
                    .SingleOrDefault(x => x.Id == id && x.IsDeleted == false);
            }

            if (invoice == null)
            {
                return NotFound();
            }

            List<InvoiceItem> invoiceItems = invoice.InvoiceItems.Where(i => i.IsDeleted == false).OrderBy(d => d.Order).ToList();

            DetailInvoiceViewModel viewModel = new DetailInvoiceViewModel
            {
                Id = invoice.Id,
                Client = invoice.Client.Name,
                InvoiceDate = invoice.InvoiceDate,
                PoNumber = invoice.PoNumber,
                GeneralWaybillNumber = invoice.GeneralWaybillNumber + invoice.BranchCode,
                BranchCode = invoice.BranchCode,
                BranchName = _context.Branches.FirstOrDefault(b => b.Code.Equals(invoice.BranchCode, StringComparison.InvariantCultureIgnoreCase))?.Name,
                Status = invoice.Status,
                InvoiceItems = invoiceItems,
                Transporter = invoice.Transporter?.Name,
                CreatedDate = invoice.CreatedDate,
                CreatedUser = invoice.CreatedUser,
                ModifiedDate = invoice.ModifiedDate,
                ModifiedUser = invoice.ModifiedUser,
                TransporterPoNumber = invoice.TransporterPoNumber,
                ShouldDownload = !string.IsNullOrWhiteSpace(shouldDownload)
            };

            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Invoice invoice = _context.Invoices.Where(x => x.Id == id && x.IsDeleted == false).Include(c => c.Client).Include(t => t.Transporter)
                .Include(i => i.InvoiceItems.Select(p => p.Product)).SingleOrDefault();

            string userId = User.GetUserId();
            Users user = _tdcContext.Users.FirstOrDefault(x => x.Id == userId && x.IsDeleted == false);

            IQueryable<Product> productQuery = _context.Products.Where(p => p.IsDeleted == false);
            IQueryable<Branch> branchQuery = _context.Branches.Where(b => b.IsDeleted == false);
            IQueryable<Client> clientQuery = _context.Clients.Where(b => b.IsDeleted == false);
            Branch branch = _context.Branches.FirstOrDefault(b => b.Code == user.BranchCode);
            if (User.IsInRole(RoleName.ThirdParty))
            {
                branchQuery = branchQuery.Where(b => b.Code.Equals(user.BranchCode));
                productQuery = productQuery.Where(p => p.Branches.Count(b => b.BranchId == branch.Id) > 0);
                clientQuery = clientQuery.Where(p => p.Branches.Count(b => b.BranchId == branch.Id) > 0);
            }

            List<Branch> branchesList = branchQuery.OrderBy(n => n.Name).ToList();
            List<Client> clientsList = clientQuery.OrderBy(d => d.Name).ToList();
            List<Product> productsList = productQuery.ToList();
            List<Transporter> transportersList = _context.Transporters.Where(b => b.IsDeleted == false).OrderBy(n => n.Name).ToList();


            if (invoice == null)
            {
                return NotFound();
            }

            EditInvoiceViewModel viewModel = new EditInvoiceViewModel
            {
                Id = invoice.Id,
                Branches = branchesList.Select(b => new EditInvoiceViewModelBranchList
                {
                    Id = b.Id,
                    Name = b.Name,
                    Code = b.Code
                }).ToList(),
                Clients = clientsList.Select(c => new EditInvoiceViewModelClientList
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList(),
                OldInvoiceItems = invoice.InvoiceItems.Where(i => i.IsDeleted == false).OrderBy(c => c.Order).Select(o => new EditInvoiceItemViewModel
                {
                    ProductName = o.Product.Name,
                    ProductId = o.ProductId,
                    Quantity = o.Quantity,
                    UnitSize = o.UnitSize,
                    TotalKg = o.TotalKg,
                    Pallets = o.Pallets,
                    BatchNumbers = o.BatchNumbers,
                }).ToList(),
                Products = productsList.Select(c => new EditInvoiceViewModelProductList
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code
                }).ToList(),
                Transporters = transportersList.Select(t => new EditInvoiceViewModelTransporterList
                {
                    Id = t.Id,
                    Name = t.Name,
                }).ToList(),
                ClientId = invoice.ClientId,
                TransporterId = invoice.TransporterId,
                InvoiceDate = invoice.InvoiceDate,
                PoNumber = invoice.PoNumber,
                GeneralWaybillNumber = invoice.GeneralWaybillNumber + invoice.BranchCode,
                BranchCode = invoice.BranchCode,
                Status = invoice.Status,
                TransporterPoNumber = invoice.TransporterPoNumber
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditInvoiceViewModel data)
        {
            if (data.Id == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid) return View(data);
            Invoice invoiceInDb = _context.Invoices.Where(x => x.Id == data.Id && !x.IsDeleted).Include(i => i.InvoiceItems).SingleOrDefault();

            if (invoiceInDb != null)
            {
                List<InvoiceItem> invoiceItemsInDb = invoiceInDb.InvoiceItems.Where(i => !i.IsDeleted).ToList();

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");


                invoiceInDb.ClientId = data.ClientId;
                invoiceInDb.TransporterId = data.TransporterId;
                invoiceInDb.InvoiceDate = data.InvoiceDate;
                invoiceInDb.PoNumber = data.PoNumber;
                invoiceInDb.BranchCode = data.BranchCode;
                invoiceInDb.Status = data.Status;
                invoiceInDb.ModifiedDate = dateTime;
                invoiceInDb.ModifiedUser = User.Identity.Name;
                invoiceInDb.TransporterPoNumber = data.TransporterPoNumber;

                Setting settings = null;

                if (invoiceInDb.GeneralWaybillNumber == null)
                {
                    switch (data.Status)
                    {
                        case "PD":
                            invoiceInDb.GeneralWaybillNumber = null;
                            break;
                        case "DL":
                        {
                            settings = _context.Settings.SingleOrDefault(x => x.Id == "4d89ab77-8a08-45d8-b5f5-ab08abca1cf6");
                            if (settings != null)
                            {
                                int waybillNumber = settings.LastWaybillNumber + 1;
                                invoiceInDb.GeneralWaybillNumber = waybillNumber.ToString();
                            }
                            else
                            {
                                invoiceInDb.GeneralWaybillNumber = null;
                            }

                            break;
                        }
                    }
                }

                foreach (InvoiceItem item in invoiceItemsInDb)
                {
                    item.ModifiedDate = dateTime;
                    item.ModifiedUser = User.Identity.Name;
                    item.DeletedDate = dateTime;
                    item.DeletedUser = User.Identity.Name;
                    item.IsDeleted = true;
                }

                for (int i = 0; i < data.InvoiceItems.Count; i++)
                {
                    InvoiceItem newInvoiceItem = new InvoiceItem
                    {
                        InvoiceId = invoiceInDb.Id,
                        ProductId = data.InvoiceItems[i].ProductId,
                        Quantity = data.InvoiceItems[i].Quantity,
                        UnitSize = data.InvoiceItems[i].UnitSize,
                        TotalKg = data.InvoiceItems[i].TotalKg,
                        Pallets = data.InvoiceItems[i].Pallets,
                        BatchNumbers = data.InvoiceItems[i].BatchNumbers,
                        CreatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time"),
                        CreatedUser = User.Identity.Name,
                        Order = i
                    };

                    _context.InvoiceItems.Add(newInvoiceItem);
                }

                _context.SaveChanges();

                if (data.Status == "DL")
                {
                    if (settings != null)
                    {
                        // Update Last Waybill Number
                        _waybillNumberService.UpdateWaybillNumber(settings.LastWaybillNumber + 1, User.Identity.Name);
                    }
                }
            }

            if (data.SaveAndDownload)
            {
                DownloadInvoiceExcel(data.Id);
            }

            return RedirectToAction("Index");
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Invoice invoice = _context.Invoices.Include(c => c.Client).Include(t => t.Transporter).Include(i => i.InvoiceItems.Select(it => it.Product))
                .SingleOrDefault(x => x.Id == id && x.IsDeleted == false);


            if (invoice == null)
            {
                return NotFound();
            }

            List<InvoiceItem> invoiceItems = invoice.InvoiceItems.Where(i => i.IsDeleted == false).OrderBy(d => d.CreatedDate).ToList();

            DeleteInvoiceViewModel viewModel = new DeleteInvoiceViewModel
            {
                Id = invoice.Id,
                Client = invoice.Client.Name,
                InvoiceDate = invoice.InvoiceDate,
                PoNumber = invoice.PoNumber,
                GeneralWaybillNumber = invoice.GeneralWaybillNumber + invoice.BranchCode,
                BranchCode = invoice.BranchCode,
                Status = invoice.Status,
                InvoiceItems = invoiceItems,
            };

            return View(viewModel);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Invoice invoiceInDb = _context.Invoices.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (invoiceInDb != null)
                {
                    invoiceInDb.ModifiedDate = dateTime;
                    invoiceInDb.ModifiedUser = User.Identity.Name;
                    invoiceInDb.DeletedDate = dateTime;
                    invoiceInDb.DeletedUser = User.Identity.Name;
                    invoiceInDb.IsDeleted = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }

            return View();
        }

        public ActionResult GetProducts(string invoiceId)
        {
            Invoice invoice = null;

            if (!string.IsNullOrWhiteSpace(invoiceId))
            {
                invoice = _context.Invoices.Where(x => x.Id == invoiceId && x.IsDeleted == false).Include(c => c.Client).Include(t => t.Transporter)
                    .Include(i => i.InvoiceItems.Select(p => p.Product)).SingleOrDefault();
            }

            List<Product> products = GetUserProducts(invoice);

            object productsList = products.Select(p => new { p.Id, p.Name, p.Code }).ToList();
            return Json(productsList);
        }

        public FileResult DownloadInvoiceExcel(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                Invoice invoice;

                if (User.IsInRole(RoleName.Admin))
                {
                    invoice = _context.Invoices.Include(c => c.Client).Include(t => t.Transporter).Include(i => i.InvoiceItems.Select(it => it.Product))
                        .SingleOrDefault(x => x.Id == id);
                }
                else
                {
                    invoice = _context.Invoices.Include(c => c.Client).Include(t => t.Transporter).Include(i => i.InvoiceItems.Select(it => it.Product))
                        .SingleOrDefault(x => x.Id == id && !x.IsDeleted);
                }

                Branch branch = _context.Branches.FirstOrDefault(b =>
                    b.Code.Equals(invoice.BranchCode, StringComparison.InvariantCultureIgnoreCase));

                if (invoice == null)
                {
                    return null;
                }

                {
                    List<InvoiceItem> invoiceItems = invoice.InvoiceItems.Where(i => i.IsDeleted == false).OrderBy(d => d.Order).ToList();

                    DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                    if (invoice != null)
                    {
                        string filePath = Path.Combine(_environment.WebRootPath, _applicationConfiguration.InvoiceTemplatePath);


                        FileInfo template = new FileInfo(filePath);

                        if (!template.Exists)
                        {
                            return null;
                        }

                        using ExcelPackage ep = new ExcelPackage(template);
                        ExcelWorksheet sheet = ep.Workbook.Worksheets.Copy("MasterSheet", "Sheet1");

                        // Fill invoice data
                        sheet.Cells["I7"].Value = invoice.GeneralWaybillNumber + invoice.BranchCode;
                        sheet.Cells["I9"].Value = invoice.PoNumber;
                        sheet.Cells["I11"].Value = invoice.InvoiceDate.Date.ToString("dd/MM/yyyy");
                        sheet.Cells["I13"].Value = _applicationConfiguration.CompanyName;
                        sheet.Cells["D13"].Value = invoice.Client.Name;

                        if (invoice.Client.Address != "None Provided")
                        {
                            sheet.Cells["D19"].Value = invoice.Client.Address;
                        }

                        if (!string.IsNullOrEmpty(invoice.TransporterId))
                        {
                            string transporterPoNumber = string.IsNullOrWhiteSpace(invoice.TransporterPoNumber) ? "" : $" ({invoice.TransporterPoNumber})";
                            sheet.Cells["D22"].Value = invoice.Transporter.Name + transporterPoNumber;
                            sheet.Cells["D24"].Value = invoice.Transporter.ContactPerson;
                            sheet.Cells["I24"].Value = invoice.Transporter.ContactNumber;
                        }

                        if (invoice.Client.ContactPersonName != "None Provided")
                        {
                            sheet.Cells["D15"].Value = invoice.Client.ContactPersonName;
                        }

                        if (invoice.Client.ContactPersonPhoneNumber != "None Provided")
                        {
                            sheet.Cells["D17"].Value = invoice.Client.ContactPersonPhoneNumber;
                        }

                        if (branch != null)
                        {
                            sheet.Cells["I15"].Value = branch.ContactPerson;
                            sheet.Cells["I17"].Value = branch.ContactNumber;
                            sheet.Cells["I19"].Value = branch.Address;
                        }

                        int row = 27;
                        int sheetCount = 1;
                        int pageCount = 1;

                        // Fill invoice items
                        foreach (InvoiceItem item in invoiceItems)
                        {
                            if (row >= 45)
                            {
                                sheetCount++;
                                pageCount++;
                                sheet = ep.Workbook.Worksheets.Copy("MasterSheet", "Sheet" + sheetCount);
                                row = 27;

                                // Fill invoice data                                    

                                sheet.Cells["I7"].Value = invoice.GeneralWaybillNumber + invoice.BranchCode;
                                sheet.Cells["I9"].Value = invoice.PoNumber;
                                sheet.Cells["I11"].Value = invoice.InvoiceDate.Date.ToString("dd/MM/yyyy");
                                sheet.Cells["I13"].Value = _applicationConfiguration.CompanyName;
                                sheet.Cells["D13"].Value = invoice.Client.Name;
                                if (invoice.Client.Address != "None Provided")
                                {
                                    sheet.Cells["D19"].Value = invoice.Client.Address;
                                }

                                if (!string.IsNullOrEmpty(invoice.TransporterId))
                                {
                                    sheet.Cells["D22"].Value = invoice.Transporter.Name;
                                    sheet.Cells["D24"].Value = invoice.Transporter.ContactPerson;
                                    sheet.Cells["I24"].Value = invoice.Transporter.ContactNumber;
                                }

                                if (invoice.Client.ContactPersonName != "None Provided")
                                {
                                    sheet.Cells["D15"].Value = invoice.Client.ContactPersonName;
                                }

                                if (invoice.Client.ContactPersonPhoneNumber != "None Provided")
                                {
                                    sheet.Cells["D17"].Value = invoice.Client.ContactPersonPhoneNumber;
                                }

                                if (branch != null)
                                {
                                    sheet.Cells["I15"].Value = branch.ContactPerson;
                                    sheet.Cells["I17"].Value = branch.ContactNumber;
                                    sheet.Cells["I19"].Value = branch.Address;
                                }
                            }

                            sheet.Cells[$"E{row}"].Value = item.Product.Name + " | " + item.Product.Code;
                            sheet.Cells[$"A{row}"].Value = item.Quantity;
                            string unitSize = item.UnitSize?.ToString("0.###");
                            if (!String.IsNullOrEmpty(unitSize))
                            {
                                sheet.Cells[$"B{row}"].Value = unitSize.Replace(",", ".");
                            }

                            sheet.Cells[$"D{row}"].Value = item.TotalKg?.ToString("0.###");
                            sheet.Cells[$"K{row}"].Value = item.Pallets;
                            if (item.BatchNumbers != null)
                            {
                                string originalString = item.BatchNumbers;

                                // See how long the batch number string is
                                if (originalString.Length > 21)
                                {
                                    string[] parts = originalString.Split(',');
                                    int rowWordCount = 0;

                                    for (int i = 0; i < parts.Length; i++)
                                    {
                                        //Start new sheet
                                        if (row >= 44)
                                        {
                                            sheetCount++;
                                            pageCount++;
                                            sheet = ep.Workbook.Worksheets.Copy("MasterSheet", "Sheet" + sheetCount);
                                            row = 27;
                                            rowWordCount = 0;

                                            // Fill invoice data                                    

                                            sheet.Cells["I7"].Value = invoice.GeneralWaybillNumber + invoice.BranchCode;
                                            sheet.Cells["I9"].Value = invoice.PoNumber;
                                            sheet.Cells["I11"].Value = invoice.InvoiceDate.Date.ToString("dd/MM/yyyy");
                                            sheet.Cells["I13"].Value = _applicationConfiguration.CompanyName;
                                            sheet.Cells["D13"].Value = invoice.Client.Name;
                                            if (invoice.Client.Address != "None Provided")
                                            {
                                                sheet.Cells["D19"].Value = invoice.Client.Address;
                                            }

                                            if (!string.IsNullOrEmpty(invoice.TransporterId))
                                            {
                                                sheet.Cells["D22"].Value = invoice.Transporter.Name;
                                                sheet.Cells["D24"].Value = invoice.Transporter.ContactPerson;
                                                sheet.Cells["I24"].Value = invoice.Transporter.ContactNumber;
                                            }

                                            if (invoice.Client.ContactPersonName != "None Provided")
                                            {
                                                sheet.Cells["D15"].Value = invoice.Client.ContactPersonName;
                                            }

                                            if (invoice.Client.ContactPersonPhoneNumber != "None Provided")
                                            {
                                                sheet.Cells["D17"].Value = invoice.Client.ContactPersonPhoneNumber;
                                            }

                                            if (branch != null)
                                            {
                                                sheet.Cells["I15"].Value = branch.ContactPerson;
                                                sheet.Cells["I17"].Value = branch.ContactNumber;
                                                sheet.Cells["I19"].Value = branch.Address;
                                            }
                                        }

                                        if (parts[i].Length > 21)
                                        {
                                            int availableSpace = 21 - rowWordCount;
                                            string start = parts[i].Substring(0, availableSpace);
                                            string end = parts[i].Substring(availableSpace, parts[i].Length - availableSpace);

                                            if (availableSpace >= start.Length)
                                            {
                                                sheet.Cells[$"J{row}"].Value = sheet.Cells[$"J{row}"].Value + start + "-";
                                                sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                row++;

                                                if (i == (parts.Length - 1))
                                                {
                                                    rowWordCount = end.Length;
                                                    sheet.Cells[$"J{row}"].Value = end;
                                                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                }
                                                else
                                                {
                                                    rowWordCount = end.Length + 1;
                                                    sheet.Cells[$"J{row}"].Value = end + ",";
                                                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                }
                                            }
                                            else
                                            {
                                                start = parts[i].Substring(0, 21);
                                                end = parts[i].Substring(21, parts[i].Length - (21 + 1));
                                                row++;
                                                sheet.Cells[$"J{row}"].Value = start + "-";
                                                sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                row++;

                                                if (i == (parts.Length - 1))
                                                {
                                                    sheet.Cells[$"J{row}"].Value = end;
                                                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    rowWordCount = end.Length;
                                                }
                                                else
                                                {
                                                    sheet.Cells[$"J{row}"].Value = end + ",";
                                                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    rowWordCount = end.Length + 1;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if ((rowWordCount + parts[i].Length) < 21)
                                            {
                                                if (sheet.Cells[$"J{row}"].Value != null)
                                                {
                                                    if (i == (parts.Length - 1))
                                                    {
                                                        sheet.Cells[$"J{row}"].Value = sheet.Cells[$"J{row}"].Value + parts[i];
                                                    }
                                                    else
                                                    {
                                                        rowWordCount = rowWordCount + parts[i].Length + 1;
                                                        sheet.Cells[$"J{row}"].Value =
                                                            sheet.Cells[$"J{row}"].Value + parts[i] + ",";
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == (parts.Length - 1))
                                                    {
                                                        rowWordCount = rowWordCount + parts[i].Length;
                                                        sheet.Cells[$"J{row}"].Value = parts[i];
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                    else
                                                    {
                                                        rowWordCount = rowWordCount + parts[i].Length + 1;
                                                        sheet.Cells[$"J{row}"].Value = parts[i] + ",";
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                row++;

                                                if (sheet.Cells[$"J{row}"].Value != null)
                                                {
                                                    if (i == (parts.Length - 1))
                                                    {
                                                        rowWordCount = parts[i].Length;
                                                        sheet.Cells[$"J{row}"].Value += parts[i];
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                    else
                                                    {
                                                        rowWordCount = parts[i].Length + 1;
                                                        sheet.Cells[$"J{row}"].Value =
                                                            sheet.Cells[$"J{row}"].Value + parts[i] + ",";
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                }
                                                else
                                                {
                                                    if (i == (parts.Length - 1))
                                                    {
                                                        rowWordCount = parts[i].Length;
                                                        sheet.Cells[$"J{row}"].Value = parts[i];
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                    else
                                                    {
                                                        rowWordCount = parts[i].Length + 1;
                                                        sheet.Cells[$"J{row}"].Value = parts[i] + ",";
                                                        sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Fits in excel block
                                    sheet.Cells[$"J{row}"].Value = originalString;
                                    sheet.Cells[$"J{row}"].Style.Numberformat.Format = "@";
                                }
                            }

                            row++;
                        }

                        ep.Workbook.Worksheets.Delete("MasterSheet");

                        int worksheetCount = 1;
                        ExcelWorksheet dispatchSheet = ep.Workbook.Worksheets["Dispatch Picking Slip Pg1"];

                        foreach (ExcelWorksheet worksheet in ep.Workbook.Worksheets)
                        {
                            if (worksheet.Name != "Dispatch Picking Slip Pg1" && worksheet.Name != "Dispatch Picking Slip Pg2")
                            {
                                worksheet.Cells["K5"].Value = "Page " + worksheetCount + " of " + pageCount;
                                worksheetCount++;
                                ep.Workbook.Worksheets.MoveBefore(worksheet.Index, dispatchSheet.Index);
                            }
                        }

                        ep.Workbook.Worksheets[0].View.ActiveCell = "I22";
                        byte[] fileContents = ep.GetAsByteArray(); // however you generate the Excel content
                        string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        string fileName = invoice.GeneralWaybillNumber + invoice.BranchCode + " - " + invoice.Client.Name + " - " +
                                          invoice.GeneralWaybillNumber +
                                          " - " + dateTime + ".xlsx";

                        return File(fileContents, contentType, fileName);
                    }
                    else
                    {
                        RedirectToAction("Index").ExecuteResult(ControllerContext);
                    }
                }
            }
            else
            {
                RedirectToAction("Index").ExecuteResult(ControllerContext);
            }


            return null;
        }

        public FileResult DownloadMonthlyReportExcel(DateTime minDate, DateTime maxDate)
        {
            _context.Database.SetCommandTimeout(200);

            List<Invoice?> invoices = _context.Invoices
                .Include(c => c.Client)
                .Include(i => i.InvoiceItems.Select(it => it.Product))
                .Where(d => d.InvoiceDate >= minDate && d.InvoiceDate <= maxDate && d.IsDeleted == false)
                .AsNoTracking()
                .OrderBy(d => d.InvoiceDate)
                .ToList();

            DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

            string filePath = Path.Combine(_environment.WebRootPath, _applicationConfiguration.ReportTemplatePath);

            FileInfo template = new FileInfo(filePath);

            if (!template.Exists)
            {
                return null;
            }


            using ExcelPackage ep = new ExcelPackage(template);
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Copy("MasterSheet", "Sheet1");

            int rowCount = 9;
            int pageCount = 1;

            foreach (Invoice item in invoices)
            {
                sheet.Cells[$"A{rowCount}"].Value = item.InvoiceDate.ToString("dd/MM/yyyy");
                sheet.Cells[$"B{rowCount}"].Value = item.InvoiceDate.DayOfWeek.ToString();
                sheet.Cells[$"B{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                sheet.Cells[$"C{rowCount}"].Value = item.Client.Name;
                sheet.Cells[$"D{rowCount}"].Value = item.PoNumber;


                sheet.Cells[$"E{rowCount}"].Value = item.BranchCode + item.GeneralWaybillNumber;
                sheet.Cells[$"E{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;


                // Filter Invoice Items for IsDeleted and sort them
                List<InvoiceItem> invoiceItemsList = item.InvoiceItems.Where(i => i.IsDeleted == false).OrderBy(d => d.Order).ToList();

                foreach (InvoiceItem invoiceItem in invoiceItemsList)
                {
                    sheet.Cells[$"A{rowCount}"].Value = item.InvoiceDate.ToString("dd/MM/yyyy");
                    sheet.Cells[$"B{rowCount}"].Value = item.InvoiceDate.DayOfWeek.ToString();
                    sheet.Cells[$"B{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"C{rowCount}"].Value = item.Client.Name;
                    sheet.Cells[$"D{rowCount}"].Value = item.PoNumber;
                    sheet.Cells[$"E{rowCount}"].Value = item.BranchCode + item.GeneralWaybillNumber;

                    sheet.Cells[$"F{rowCount}"].Value = invoiceItem.Product.Name + " | " + invoiceItem.Product.Code;
                    sheet.Cells[$"G{rowCount}"].Value = invoiceItem.Quantity;
                    sheet.Cells[$"H{rowCount}"].Value = invoiceItem.UnitSize;
                    sheet.Cells[$"I{rowCount}"].Value = invoiceItem.TotalKg;
                    sheet.Cells[$"J{rowCount}"].Value = invoiceItem.Pallets;

                    sheet.Cells[$"G{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"H{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"I{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"J{rowCount}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    if (invoiceItem.BatchNumbers != null)
                    {
                        string originalString = invoiceItem.BatchNumbers;
                        sheet.Cells[$"K{rowCount}"].Value = originalString;
                        sheet.Cells[$"K{rowCount}"].Style.WrapText = true;
                    }

                    rowCount++;
                }
            }

            ep.Workbook.Worksheets.Delete("MasterSheet");

            int worksheetCount = 1;

            foreach (ExcelWorksheet worksheet in ep.Workbook.Worksheets)
            {
                worksheet.Cells["K5"].Value = "Page " + worksheetCount + " of " + pageCount;
                worksheetCount++;
            }

            ep.Workbook.Worksheets[0].View.ActiveCell = "I22";
            byte[] fileContents = ep.GetAsByteArray(); // however you generate the Excel content
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Delivery-Notes-Report-" + dateTime + ".xlsx";

            return File(fileContents, contentType, fileName);
        }


        private List<Product> GetUserProducts(Invoice invoice = null)
        {
            string userId = User.GetUserId();
            Users user = _tdcContext.Users.FirstOrDefault(x => x.Id == userId && x.IsDeleted == false);
            Branch branch = _context.Branches.FirstOrDefault(b => b.Code == user.BranchCode);
            IQueryable<Product> productQuery = _context.Products.Where(p => p.IsDeleted == false);

            if (User.IsInRole(RoleName.ThirdParty))
            {
                productQuery = productQuery.Where(p => p.Branches.Count(b => b.BranchId == branch.Id) > 0);
            }

            List<Product> products = productQuery.ToList();

            if (invoice != null && User.IsInRole(RoleName.ThirdParty))
            {
                products.AddRange(invoice.InvoiceItems.Select(i => i.Product).Distinct());
            }

            return products;
        }
    }
}