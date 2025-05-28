using InvoiceGenerator2.Models;
using InvoiceGenerator2.Models.ViewModels.Branches;
using InvoiceGenerator2.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator2.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class BranchesController : Controller
    {
        private ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        // GET
        public ActionResult Index()
        {
            return View(_context.Branches.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
        }
        
        public ViewResult Create()
        {
            return View();
        }
        
        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBranchViewModel branch)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                Branch newBranch = new Branch();

                newBranch.Code = branch.Code;
                newBranch.Name = branch.Name;
                newBranch.ContactPerson = branch.ContactPerson;
                newBranch.ContactNumber = branch.ContactNumber;
                newBranch.Address = branch.Address;
                newBranch.CreatedDate = dateTime;
                newBranch.CreatedUser = User.Identity.Name;

                _context.Branches.Add(newBranch);
                _context.SaveChanges();

                return RedirectToAction("Index");
                
            }

            return View(branch);
        }
        
        // GET: Branches/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Branch product = _context.Branches.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (product == null)
            {
                return NotFound();
            }

            DetailBranchViewModel viewModel = new DetailBranchViewModel()
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                ContactPerson = product.ContactPerson,
                ContactNumber = product.ContactNumber,
                Address = product.Address,
                CreatedDate = product.CreatedDate,
                CreatedUser = product.CreatedUser,
                ModifiedDate = product.ModifiedDate,
                ModifiedUser = product.ModifiedUser,
            };

            return View(viewModel);
        }
        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Branch product = _context.Branches.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (product == null)
            {
                return NotFound();
            }

            EditBranchViewModel viewModel = new EditBranchViewModel()
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                ContactPerson = product.ContactPerson,
                ContactNumber = product.ContactNumber,
                Address = product.Address
            };

            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditBranchViewModel data)
        {
            if (data.Id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {

                Branch branchInDb = _context.Branches.SingleOrDefault(x => x.Id == data.Id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (branchInDb != null)
                {
                    branchInDb.Code = data.Code;
                    branchInDb.Name = data.Name;
                    branchInDb.ContactPerson = data.ContactPerson;
                    branchInDb.ContactNumber = data.ContactNumber;
                    branchInDb.Address = data.Address;
                    branchInDb.ModifiedDate = dateTime;
                    branchInDb.ModifiedUser = User.Identity.Name;
                    _context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }

            }
            return View(data);
        }
        
        // GET: Branches/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Branch branch = _context.Branches.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (branch == null)
            {
                return NotFound();
            }

            DeleteBranchViewModel viewModel = new DeleteBranchViewModel()
            {
                Id = branch.Id,
                Code = branch.Code,
                Name = branch.Name,
                ContactPerson = branch.ContactPerson,
                ContactNumber = branch.ContactNumber,
                Address = branch.Address
            };

            return View(viewModel);
        }
        
        // POST: Branches/Delete/5
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

                Branch branchInDb = _context.Branches.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (branchInDb != null)
                {
                    branchInDb.ModifiedDate = dateTime;
                    branchInDb.ModifiedUser = User.Identity.Name;
                    branchInDb.DeletedDate = dateTime;
                    branchInDb.DeletedUser = User.Identity.Name;
                    branchInDb.IsDeleted = true;
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
    }
}