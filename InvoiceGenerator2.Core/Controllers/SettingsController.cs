using InvoiceGenerator2.Models;
using InvoiceGenerator2.Models.ViewModels.Settings;
using InvoiceGenerator2.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceGenerator2.Core.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public SettingsController(ApplicationDbContext context, ApplicationConfiguration applicationConfiguration)
        {
            _context = context;
            _applicationConfiguration = applicationConfiguration;
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [HttpGet]
        public ActionResult WaybillSettings()
        {
            Setting setting = _context.Settings.SingleOrDefault(x => x.Id == _applicationConfiguration.ConfigKey);

            if (setting == null)
            {
                return NotFound();
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

                Setting settingInDb = _context.Settings.SingleOrDefault(x => x.Id == _applicationConfiguration.ConfigKey);

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
                    return NotFound();
                }

            }
            return View(data);
        }
    }
}