using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using TDC_Invoice_Generator.Models;
using TDC_Invoice_Generator.ViewModels.Transporters;

namespace TDC_Invoice_Generator.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class TransportersController : Controller
    {
        
        private ApplicationDbContext _context;

        public TransportersController()
        {
            _context = new ApplicationDbContext();
        }
        
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        
        // GET
        public ActionResult Index()
        {
            return View(_context.Transporters.Where(u => u.IsDeleted == false).OrderByDescending(d => d.CreatedDate).ToList());
        }
        
        public ViewResult Create()
        {
            return View();
        }
        
        // POST: Transporters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTransporterViewModel transporter)
        {
            if (ModelState.IsValid)
            {
                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                Transporter newTransporter = new Transporter();
                
                newTransporter.Name = transporter.Name;
                newTransporter.ContactPerson = transporter.ContactPerson;
                newTransporter.ContactNumber = transporter.ContactNumber;
                newTransporter.CreatedDate = dateTime;
                newTransporter.CreatedUser = User.Identity.Name;

                _context.Transporters.Add(newTransporter);
                _context.SaveChanges();

                return RedirectToAction("Index");
                
            }

            return View(transporter);
        }
        
        // GET: Branches/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return HttpNotFound();
            }

            DetailTransporterViewModel viewModel = new DetailTransporterViewModel()
            {
                Id = transporter.Id,
                Name = transporter.Name,
                ContactPerson = transporter.ContactPerson,
                ContactNumber = transporter.ContactNumber,
                CreatedDate = transporter.CreatedDate,
                CreatedUser = transporter.CreatedUser,
                ModifiedDate = transporter.ModifiedDate,
                ModifiedUser = transporter.ModifiedUser,
            };

            return View(viewModel);
        }
        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return HttpNotFound();
            }

            EditTransporterViewModel viewModel = new EditTransporterViewModel()
            {
                Id = transporter.Id,
                Name = transporter.Name,
                ContactPerson = transporter.ContactPerson,
                ContactNumber = transporter.ContactNumber
            };

            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditTransporterViewModel data)
        {
            if (data.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {

                Transporter transporterInDb = _context.Transporters.SingleOrDefault(x => x.Id == data.Id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (transporterInDb != null)
                {
                    transporterInDb.Name = data.Name;
                    transporterInDb.ContactPerson = data.ContactPerson;
                    transporterInDb.ContactNumber = data.ContactNumber;
                    transporterInDb.ModifiedDate = dateTime;
                    transporterInDb.ModifiedUser = User.Identity.Name;
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
        
        // GET: Transporters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return HttpNotFound();
            }

            DeleteTransporterViewModel viewModel = new DeleteTransporterViewModel()
            {
                Id = transporter.Id,
                Name = transporter.Name,
                ContactPerson = transporter.ContactPerson,
                ContactNumber = transporter.ContactNumber
            };

            return View(viewModel);
        }
        
        // POST: Transporters/Delete/5
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

                Transporter transporterInDb = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (transporterInDb != null)
                {

                    // Remove this transporter from all invoices
                    List<Invoice> invoicesWithTransporter =
                        _context.Invoices.Where(x => x.TransporterId.Equals(transporterInDb.Id)).ToList();
                    
                    foreach(Invoice invoice in invoicesWithTransporter)
                    {
                        invoice.TransporterId = null;
                    }

                    transporterInDb.ModifiedDate = dateTime;
                    transporterInDb.ModifiedUser = User.Identity.Name;
                    transporterInDb.DeletedDate = dateTime;
                    transporterInDb.DeletedUser = User.Identity.Name;
                    transporterInDb.IsDeleted = true;
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