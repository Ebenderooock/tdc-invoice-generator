using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using TDC_Invoice_Generator.Helper;
using TDC_Invoice_Generator.Models;
using TDC_Invoice_Generator.ViewModels.Products;
using TDC_Invoice_Generator.ViewModels.Users;

namespace TDC_Invoice_Generator.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TDCDbContext _tdcContext;

        public ProductsController()
        {
            _context = new ApplicationDbContext();
            _tdcContext = new TDCDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Products
        public ActionResult Index()
        {
            return View(_context.Products.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
        }

        public ViewResult Create()
        {
            CreateProductViewModel product = new CreateProductViewModel();

            string userId = User.Identity.GetUserId();
            Users user = _tdcContext.Users.FirstOrDefault(x => x.Id == userId && x.IsDeleted == false);

            List<Branch> branches = _context.Branches.ToList();

            product.Branches = new List<SelectListItem>() { };

            if (User.IsInRole(RoleName.ThirdParty))
            {
                var branch = branches.FirstOrDefault(b => b.Code == user.BranchCode);
                product.Branches = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = branch.Id,
                        Text = branch.Name,
                        Selected = true
                    }
                };
            }
            else
            {
                product.Branches = branches.Select(b => new SelectListItem
                {
                    Value = b.Id,
                    Text = b.Name,
                    Selected = true
                }).ToList();
            }

            return View(product);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Check for existing product by code
                    Product existingProduct = _context.Products.FirstOrDefault(p => p.Code.Equals(product.Code, StringComparison.InvariantCultureIgnoreCase));

                    if (existingProduct != null)
                    {
                        ModelState.AddModelError("Code", @"Product code already exists");
                        return View(product);
                    }

                    DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                    Product newProduct = new Product();
                    newProduct.Id = Guid.NewGuid().ToString();
                    newProduct.Code = product.Code;
                    newProduct.Branches = product.SelectedBranches.Select(b => new ProductBranch { Id = Guid.NewGuid().ToString(), ProductId = newProduct.Id, BranchId = b }).ToList();
                    newProduct.Name = product.Name;
                    newProduct.CreatedDate = dateTime;
                    newProduct.CreatedUser = User.Identity.Name;

                    _context.Products.Add(newProduct);
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(product);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _context.Products.Include(p => p.Branch).SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (product == null)
            {
                return HttpNotFound();
            }

            DetailProductViewModel viewModel = new DetailProductViewModel
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                BranchName = product.Branch?.Name ?? "All",
                CreatedDate = product.CreatedDate,
                CreatedUser = product.CreatedUser,
                ModifiedDate = product.ModifiedDate,
                ModifiedUser = product.ModifiedUser
            };

            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _context.Products.Include("Branches").SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (product == null)
            {
                return HttpNotFound();
            }

            EditProductViewModel viewModel = new EditProductViewModel
            {
                Id = product.Id,
                Code = product.Code,
                SelectedBranches = product.Branches.Select(b => b.BranchId).ToList(),
                Name = product.Name
            };

            string userId = User.Identity.GetUserId();
            Users user = _tdcContext.Users.FirstOrDefault(x => x.Id == userId && x.IsDeleted == false);

            List<Branch> branches = _context.Branches.ToList();


            if (User.IsInRole(RoleName.ThirdParty))
            {
                var branch = branches.FirstOrDefault(b => b.Code == user.BranchCode);
                viewModel.Branches = new List<SelectListItem>()
                {
                    new SelectListItem
                    {
                        Value = branch.Id,
                        Text = branch.Name,
                        Selected = true
                    }
                };
            }
            else
            {
                viewModel.Branches = branches.Select(b => new SelectListItem
                {
                    Value = b.Id,
                    Text = b.Name,
                    Selected = true
                }).ToList();
            }



            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditProductViewModel data, string[] assignments)
        {
            if (data.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                //Check for existing product by code
                Product existingProduct = _context.Products.FirstOrDefault(p =>
                    p.Code.Equals(data.Code, StringComparison.InvariantCultureIgnoreCase) && !p.Id.Equals(data.Id));

                if (existingProduct != null)
                {
                    ModelState.AddModelError("Code", @"Product code already exists");
                    return View(data);
                }

                Product productInDb = _context.Products.SingleOrDefault(x => x.Id == data.Id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (productInDb != null)
                {
                    productInDb.Code = data.Code;
                    productInDb.Name = data.Name;
                    productInDb.Branches = data.SelectedBranches.Select(b => new ProductBranch { Id = Guid.NewGuid().ToString(), BranchId = b}).ToList();
                    productInDb.ModifiedDate = dateTime;
                    productInDb.ModifiedUser = User.Identity.Name;



                    var entriesToDelete = _context.ProductBranches.Where(cb => cb.ProductId == productInDb.Id && !data.SelectedBranches.Contains(cb.Id));
                    _context.ProductBranches.RemoveRange(entriesToDelete);

                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return HttpNotFound();
            }

            return View(data);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Product product = _context.Products.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (product == null)
            {
                return HttpNotFound();
            }

            DeleteProductViewModel viewModel = new DeleteProductViewModel
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name
            };

            return View(viewModel);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                Product productInDb = _context.Products.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (productInDb != null)
                {
                    productInDb.ModifiedDate = dateTime;
                    productInDb.ModifiedUser = User.Identity.Name;
                    productInDb.DeletedDate = dateTime;
                    productInDb.DeletedUser = User.Identity.Name;
                    productInDb.IsDeleted = true;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }

                return HttpNotFound();
            }

            return View();
        }

        [HttpGet]
        public ViewResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(UploadProductViewModel data)
        {
            if (data.File != null)
            {
                string filename = data.File.FileName;
                if (filename.EndsWith(".xls") || filename.EndsWith(".xlsx"))
                {
                    if (data.File.ContentLength > 1024 * 1024 * 50) // 50MB limit  
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

                    List<Product> productList = new List<Product>();

                    FileInfo existingFile = new FileInfo(filePath);
                    List<Branch> existingBranches = _context.Branches.Where(b => b.IsDeleted == false).ToList();
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //Get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        int range = worksheet.Dimension.End.Row + 1;

                        for (int i = 1; i < range; i++)
                        {
                            if (worksheet.Cells[i, 1].Value != null && (string)worksheet.Cells[i, 1].Value != "")
                            {
                                Product productData = new Product();
                                productData.Code = worksheet.Cells[i, 1].Value.ToString().Trim();
                                productData.Name = worksheet.Cells[i, 2].Value.ToString().Trim();
                                productData.CreatedDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                                productData.CreatedUser = User.Identity.Name;
                                productList.Add(productData);
                            }
                        }
                    } // the using statement automatically calls Dispose() which closes the package.

                    FileInfo file = new FileInfo(filePath);
                    file.Delete();

                    List<string> productCodes = productList.Select(p => p.Code).ToList();
                    // Update existing products
                    List<Product> dbProducts = _context.Products.ToList().Where(x => productCodes.Any(p => p.Equals(x.Code))).ToList();
                    dbProducts.ForEach(p =>
                    {
                        Product excelProduct = productList.First(e => e.Code.Equals(p.Code));

                        p.Name = excelProduct.Name;
                        p.IsDeleted = excelProduct.IsDeleted;
                        p.Branches = existingBranches.Select(b => new ProductBranch { Id = Guid.NewGuid().ToString(), BranchId = b.Id }).ToList();
                    });

                    // Add new products
                    productList.Where(e => dbProducts.All(p => !p.Code.Equals(e.Code))).ToList().ForEach(e => { _context.Products.Add(e); });

                    _context.SaveChanges();

                    return Redirect("/Products");
                }

                ModelState.AddModelError("", @"Invalid File Type");
                return View();
            }

            return View();
        }
    }
}