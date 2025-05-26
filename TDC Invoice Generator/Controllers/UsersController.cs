using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using InvoiceGenerator_Core.Models;
using InvoiceGenerator_Core.ViewModels.Users;

namespace InvoiceGenerator_Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class UsersController : Controller
    {
        private readonly TDCDbContext _context = new TDCDbContext();
        private readonly ApplicationDbContext _applicationDbContext = new ApplicationDbContext();

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Users
        public ViewResult Index()
        {
            return View(_context.Users.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
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
        public ActionResult Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = new ApplicationUser
                {
                    FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, PhoneNumber = model.PhoneNumber, BranchCode = model.BranchCode,
                    Email = model.Email, CreatedUser = User.Identity.Name, CreatedDate = dateTime
                };
                IdentityResult result = manager.Create(user, model.Password);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = _context.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            SqlParameter userId = new SqlParameter("UserID", id);
            List<Role> roles = _context.Database
                .SqlQuery<Role>(
                    "SELECT r.Id, r.Name, IIF(i.UserId = @UserId,CAST(1 AS BIT),CAST(0 AS BIT)) Assigned FROM AspNetRoles r  WITH (NOLOCK) LEFT JOIN AspNetUserRoles i  WITH (NOLOCK) ON r.Id = i.RoleId AND i.UserId = @UserID",
                    userId).ToList();

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Users user = _context.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            List<string> roleAssignments = _applicationDbContext.Users.First(u => u.Id == id).Roles.Select(r => r.RoleId).ToList();
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                Users userInDb = _context.Users.Where(x => x.Id == data.Id && x.IsDeleted == false).SingleOrDefault();

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
                    _context.SaveChanges();

                    if (!string.IsNullOrEmpty(data.Password))
                    {
                        ApplicationUserManager manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        manager.RemovePassword(data.Id);
                        manager.AddPassword(data.Id, data.Password);
                    }

                    SqlParameter userId = new SqlParameter("UserID", data.Id);
                    StringBuilder command = new StringBuilder("DELETE FROM AspNetUserRoles WHERE UserId = @UserID;");

                    if (assignments != null && assignments.Count() > 0)
                    {
                        command.Append("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES " +
                                       "('" + data.Id + "','" + string.Join("'),('" + data.Id + "','", assignments) + "')");
                    }

                    _context.Database.ExecuteSqlCommand(command.ToString(), userId);


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

            Users user = _context.Users.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (user == null)
            {
                return HttpNotFound();
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
    }
}