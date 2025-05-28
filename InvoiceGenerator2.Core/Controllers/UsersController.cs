using InvoiceGenerator2.Models;
using InvoiceGenerator2.Models.ViewModels.Users;
using InvoiceGenerator2.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Role = InvoiceGenerator2.Models.ViewModels.Users.Role;
using Users = InvoiceGenerator2.Models.Users;

namespace InvoiceGenerator2.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TDCDbContext _context ;
        private readonly ApplicationDbContext _applicationDbContext ;

        public UsersController(ApplicationDbContext context, TDCDbContext tdcDbContext, UserManager<ApplicationUser> userManager)
        {
            _applicationDbContext = context;
            _context = tdcDbContext;
            _userManager = userManager;
        }

        protected override void Dispose(bool disposing)
        {
            _applicationDbContext.Dispose();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        // GET: Users
        public ViewResult Index()
        {
            return View(_applicationDbContext.Users.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
        }

        public ViewResult Create()
        {
            CreateUserViewModel viewModel = new CreateUserViewModel
            {
                BranchCodes = _applicationDbContext.Branches.Select(b => new SelectListItem
                {
                    Text = b.Name, Value = b.Code
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, PhoneNumber = model.PhoneNumber, BranchCode = model.BranchCode,
                    Email = model.Email, CreatedUser = User.Identity.Name, CreatedDate = dateTime
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Edit", "Users", new { id = user.Id });
                }

                AddErrors(result);
            }

            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Users user = _context.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            List<Role> roles = _applicationDbContext.Database
                .SqlQuery<Role>(
                    $"SELECT r.Id, r.Name, IIF(i.UserId = @UserId,CAST(1 AS BIT),CAST(0 AS BIT)) Assigned FROM AspNetRoles r  WITH (NOLOCK) LEFT JOIN AspNetUserRoles i  WITH (NOLOCK) ON r.Id = i.RoleId AND i.UserId = {id}")
                .ToList();

            DetailUserViewModel viewModel = new DetailUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BranchCode = user.BranchCode,
                CreatedDate = user.CreatedDate,
                CreatedUser = user.CreatedUser,
                ModifiedDate = user.ModifiedDate,
                ModifiedUser = user.ModifiedUser,
                Roles = roles
            };

            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Users user = _context.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            List<string> roleAssignments = _context.Users.First(u => u.Id == id).Roles.Select(r => r.Id).ToList();
            List<Role> roles = _applicationDbContext.Roles.Select(r =>
                new Role
                {
                    Id = r.Id,
                    Name = r.Name,
                    Assigned = roleAssignments.Contains(r.Id)
                }
            ).ToList();

            EditUserViewModel viewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BranchCode = user.BranchCode,
                Roles = roles,
                BranchCodes = _applicationDbContext.Branches.Select(b => new SelectListItem
                {
                    Text = b.Name, Value = b.Code
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditUserViewModel data, string[] assignments)
        {
            if (data.Id == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                Users userInDb = _context.Users.Where(x => x.Id == data.Id && x.IsDeleted == false).SingleOrDefault();
                ApplicationUser user  = _userManager.FindByIdAsync(data.Id).Result;
                if (userInDb != null)
                {
                    DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                    userInDb.FirstName = data.FirstName;
                    userInDb.Email = data.Email;
                    userInDb.LastName = data.LastName;
                    userInDb.PhoneNumber = data.PhoneNumber;
                    userInDb.BranchCode = data.BranchCode;
                    userInDb.ModifiedDate = dateTime;
                    userInDb.ModifiedUser = User.Identity.Name;
                    _applicationDbContext.SaveChanges();

                    if (!string.IsNullOrEmpty(data.Password))
                    {
                        _userManager.RemovePasswordAsync(user);
                        _userManager.AddPasswordAsync(user, data.Password);
                    }

                    string command = new string($"DELETE FROM AspNetUserRoles WHERE UserId = {data.Id};");

                    if (assignments != null && assignments.Count() > 0)
                    {
                        command += $"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES " + "('" + data.Id + "','" + string.Join("'),('" + data.Id + "','", assignments) + "')";
                    }

                    _applicationDbContext.Database.ExecuteSqlRaw(command);


                    return RedirectToAction("Index");
                }

                return NotFound();
            }

            return View(data);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Users user = _context.Users.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (user == null)
            {
                return NotFound();
            }

            DeleteUserViewModel viewModel = new DeleteUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BranchCode = user.BranchCode,
            };

            return View(viewModel);
        }

        // POST: Users/Delete/5
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
                Users userInDb = _context.Users.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

                if (userInDb != null)
                {
                    DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                    userInDb.ModifiedDate = dateTime;
                    userInDb.ModifiedUser = User.Identity.Name;
                    userInDb.DeletedDate = dateTime;
                    userInDb.DeletedUser = User.Identity.Name;
                    userInDb.IsDeleted = true;
                    _applicationDbContext.SaveChanges();

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