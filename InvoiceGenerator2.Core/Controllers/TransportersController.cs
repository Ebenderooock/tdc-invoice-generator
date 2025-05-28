using InvoiceGenerator2.Models;
using InvoiceGenerator2.Models.ViewModels.Transporters;
using InvoiceGenerator2.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator2.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class TransportersController : Controller
    {
        
        private ApplicationDbContext _context;

        public TransportersController(ApplicationDbContext context)
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
                return BadRequest();
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return NotFound();
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
                return BadRequest();
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return NotFound();
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
                return BadRequest();
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
                    return NotFound();
                }

            }
            return View(data);
        }
        
        // GET: Transporters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Transporter transporter = _context.Transporters.SingleOrDefault(x => x.Id == id && x.IsDeleted == false);

            if (transporter == null)
            {
                return NotFound();
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
                return BadRequest();
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
                    return NotFound();
                }

            }
            return View();
        }
    }
}