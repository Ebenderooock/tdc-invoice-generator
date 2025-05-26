using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using InvoiceGenerator.Core.Models;
using InvoiceGenerator.Core.ViewModels.Clients;

namespace InvoiceGenerator.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class ClientsController : Controller
    {
        private ApplicationDbContext _context;

        public ClientsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Clients
        public ActionResult Index()
        {
            return View(_context.Clients.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
        }

        public ViewResult Create()
        {
            return View(new CreateClientViewModel()
            {
                Branches = _context.Branches.Where(b => b.IsDeleted == false).Select(b => new SelectListItem { Value = b.Id, Text = b.Name, Selected = true }).ToList()
            });
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateClientViewModel client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                    Client newClient = new Client();
                    newClient.Id = Guid.NewGuid().ToString();
                    newClient.AccountNumber = client.AccountNumber;
                    newClient.Name = client.Name;
                    newClient.Address = client.Address;
                    newClient.ContactPersonName = client.ContactPersonName;
                    newClient.ContactPersonPhoneNumber = client.ContactPersonPhoneNumber;
                    newClient.CreatedDate = dateTime;
                    newClient.CreatedUser = User.Identity.Name;

                    newClient.Branches = client.SelectedBranches.Select(b => new ClientBranch { Id = Guid.NewGuid().ToString(), BranchId = b, ClientId = newClient.Id }).ToList();

                    _context.Clients.Add(newClient);
                    _context.SaveChanges();

                    return RedirectToAction("Index");

                }

                return View(client);
            }
            catch (Exception exception)
            {

                return View(client);
            }
        }

        // GET: Clients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = _context.Clients.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (client == null)
            {
                return HttpNotFound();
            }

            DetailClientViewModel viewModel = new DetailClientViewModel
            {
                Id = client.Id,
                AccountNumber = client.AccountNumber,
                Name = client.Name,
                Address = client.Address,
                ContactPersonName = client.ContactPersonName,
                ContactPersonPhoneNumber = client.ContactPersonPhoneNumber,
                CreatedDate = client.CreatedDate,
                CreatedUser = client.CreatedUser,
                ModifiedDate = client.ModifiedDate,
                ModifiedUser = client.ModifiedUser,
                Branches = client.Branches.Select(b => b.Branch.Name).ToList()
            };

            return View(viewModel);

        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = _context.Clients.Include("Branches").Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (client == null)
            {
                return HttpNotFound();
            }

            EditClientViewModel viewModel = new EditClientViewModel
            {
                Id = client.Id,
                AccountNumber = client.AccountNumber,
                Name = client.Name,
                Address = client.Address,
                ContactPersonName = client.ContactPersonName,
                ContactPersonPhoneNumber = client.ContactPersonPhoneNumber,
                Branches = _context.Branches.Where(b => b.IsDeleted == false).Select(b => new SelectListItem { Value = b.Id, Text = b.Name, Selected = true }).ToList(),
                SelectedBranches = client.Branches.Select(b => b.BranchId)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditClientViewModel data)
        {
            if (data.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {

                Client clientInDb = _context.Clients.Where(x => x.Id == data.Id && x.IsDeleted == false).SingleOrDefault();

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");


                if (clientInDb != null)
                {
                    clientInDb.AccountNumber = data.AccountNumber;
                    clientInDb.Name = data.Name;
                    clientInDb.Address = data.Address;
                    clientInDb.ContactPersonName = data.ContactPersonName;
                    clientInDb.ContactPersonPhoneNumber = data.ContactPersonPhoneNumber;
                    clientInDb.ModifiedDate = dateTime;
                    clientInDb.ModifiedUser = User.Identity.Name;
                    clientInDb.Branches = data.SelectedBranches.Select(b => new ClientBranch { Id= Guid.NewGuid().ToString(), BranchId = b }).ToList();

                    var entriesToDelete = _context.ClientBranches.Where(cb => cb.ClientId == clientInDb.Id && !data.SelectedBranches.Contains(cb.Id));
                    _context.ClientBranches.RemoveRange(entriesToDelete);

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }

            }
            return View(data);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Client client = _context.Clients.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (client == null)
            {
                return HttpNotFound();
            }

            DeleteClientViewModel viewModel = new DeleteClientViewModel
            {
                Id = client.Id,
                AccountNumber = client.AccountNumber,
                Name = client.Name,
                Address = client.Address,
                ContactPersonName = client.ContactPersonName,
                ContactPersonPhoneNumber = client.ContactPersonPhoneNumber,
            };

            return View(viewModel);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {

                Client clientInDb = _context.Clients.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (clientInDb != null)
                {
                    clientInDb.ModifiedDate = dateTime;
                    clientInDb.ModifiedUser = User.Identity.Name;
                    clientInDb.DeletedDate = dateTime;
                    clientInDb.DeletedUser = User.Identity.Name;
                    clientInDb.IsDeleted = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }

            }
            return View();
        }

        [HttpGet]
        public ViewResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(UploadClientViewModel data)
        {
            if (data.File != null)
            {
                string filename = data.File.FileName;
                if (filename.EndsWith(".xls") || filename.EndsWith(".xlsx"))
                {
                    if (data.File.ContentLength > (1024 * 1024 * 50))  // 50MB limit  
                    {
                        ModelState.AddModelError("postedFile", @"Your file is to large. Maximum size allowed is 50MB!");
                    }

                    string path = Server.MapPath("~/TempUploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string filePath = path + Path.GetFileName(data.File.FileName);
                    data.File.SaveAs(filePath);

                    // Loop through document and get values
                    List<Client> clientList = new List<Client>();
                    FileInfo existingFile = new FileInfo(filePath);
                    List<Branch> existingBranches = _context.Branches.Where(b => b.IsDeleted == false).ToList();
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //Get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        int range = worksheet.Dimension.End.Row + 1;

                        for (int i = 1; i < range; i++)
                        {
                            if (worksheet.Cells[i, 1].Value.ToString() == "Acc" && worksheet.Cells[i, 1].Value != null && worksheet.Cells[i, 1].Value != "")
                            {
                                int currentRow = i;

                                Client newClient = new Client();
                                newClient.AccountNumber = (string)worksheet.Cells[currentRow, 2].Value;
                                newClient.Name = (string)worksheet.Cells[currentRow + 1, 2].Value;

                                // Generate address
                                string address = "";

                                if (worksheet.Cells[currentRow + 3, 2].Value.ToString() != "")
                                {
                                    address += worksheet.Cells[currentRow + 3, 2].Value + " ";
                                }

                                if (worksheet.Cells[currentRow + 4, 2].Value.ToString() != "")
                                {
                                    address += worksheet.Cells[currentRow + 4, 2].Value + " ";
                                }

                                if (worksheet.Cells[currentRow + 5, 2].Value.ToString() != "")
                                {
                                    address += worksheet.Cells[currentRow + 5, 2].Value + " ";
                                }

                                if (worksheet.Cells[currentRow + 6, 2].Value.ToString() != "")
                                {
                                    address += worksheet.Cells[currentRow + 6, 2].Value + " ";
                                }

                                if (!string.IsNullOrEmpty(address))
                                {
                                    newClient.Address = address;
                                }
                                else
                                {
                                    newClient.Address = "None Provided";
                                }

                                // Check if item has Contact PersonName
                                if (worksheet.Cells[currentRow + 2, 4].Value.ToString() != "")
                                {
                                    newClient.ContactPersonName = (string)worksheet.Cells[currentRow + 2, 4].Value;
                                }
                                else
                                {
                                    newClient.ContactPersonName = "None Provided";
                                }

                                // Check if item has Contact Person Phone Number
                                if (worksheet.Cells[currentRow, 4].Value.ToString() != "")
                                {
                                    newClient.ContactPersonPhoneNumber = worksheet.Cells[currentRow, 4].Value.ToString();
                                }
                                else
                                {
                                    if (worksheet.Cells[currentRow + 3, 4].Value.ToString() != "")
                                    {
                                        newClient.ContactPersonPhoneNumber = worksheet.Cells[currentRow + 3, 4].Value.ToString();
                                    }
                                    else
                                    {
                                        newClient.ContactPersonPhoneNumber = "None Provided";
                                    }
                                }

                                newClient.CreatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time"); ;
                                newClient.CreatedUser = User.Identity.Name;
                                newClient.Branches = existingBranches.Select(b => new ClientBranch { Id = Guid.NewGuid().ToString(), BranchId = b.Id }).ToList();
                                clientList.Add(newClient);
                            }
                        }

                    }

                    System.IO.File.Delete(filePath);

                    try
                    {
                        _context.Clients.AddOrUpdate(x => x.AccountNumber, clientList.ToArray());
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        return View();
                    }


                    return Redirect("/Clients");
                }

                ModelState.AddModelError("", @"Invalid File Type");
                return View();
            }

            return View();
        }
    }
}