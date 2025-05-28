using InvoiceGenerator2.Models;
using InvoiceGenerator2.Persistence;

namespace InvoiceGenerator2.Helpers
{
    public class WaybillNumberService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationConfiguration _applicationConfiguration;

        public WaybillNumberService(ApplicationDbContext context, ApplicationConfiguration applicationConfiguration)
        {
            _context = context;
            _applicationConfiguration = applicationConfiguration;
        }
        
        public void UpdateWaybillNumber(int lastWaybillNumber, string modifiedUser)
        {
            Setting? settings = _context.Settings.SingleOrDefault(x => x.Id == _applicationConfiguration.ConfigKey);
            
            DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

            if (settings == null)
            {
                return;
            }

            settings.LastWaybillNumber = lastWaybillNumber;
            settings.ModifiedDate = dateTime;
            settings.ModifiedUser = modifiedUser;
            _context.SaveChanges();
        }
    }
}