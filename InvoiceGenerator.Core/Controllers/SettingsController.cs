using System;
using System.Linq;
using System.Web.Mvc;
using InvoiceGenerator.Core.Helper;
using InvoiceGenerator.Core.Models;
using InvoiceGenerator.Core.ViewModels.Settings;

namespace InvoiceGenerator.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SettingsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public ActionResult WaybillSettings()
        {
            Setting setting = _context.Settings.SingleOrDefault(x => x.Id == Settings.ConfigKey);

            if (setting == null)
            {
                return HttpNotFound();
            }

            WaybillSettingtViewModel viewModel = new WaybillSettingtViewModel
            {
                Id = setting.Id,
                LastWaybillNumber = setting.LastWaybillNumber,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WaybillSettings(WaybillSettingtViewModel data)
        {
            if (ModelState.IsValid)
            {

                Setting settingInDb = _context.Settings.SingleOrDefault(x => x.Id == Settings.ConfigKey);

                DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

                if (settingInDb != null)
                {
                    settingInDb.LastWaybillNumber = data.LastWaybillNumber;
                    settingInDb.ModifiedDate = dateTime;
                    settingInDb.ModifiedUser = User.Identity.Name;
                    _context.SaveChanges();

                    return Redirect("/");
                }
                else
                {
                    return HttpNotFound();
                }

            }
            return View(data);
        }
    }
}