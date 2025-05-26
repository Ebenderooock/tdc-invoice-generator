using System;
using System.Linq;
using InvoiceGenerator_Core.Models;

namespace InvoiceGenerator_Core.Helper
{
    public class WaybillNumberHelper
    {
        public static void UpdateWaybillNumber(int lastWaybillNumber, string modifiedUser)
        {
            ApplicationDbContext context = new ApplicationDbContext();

            Setting settings = context.Settings.SingleOrDefault(x => x.Id == Settings.ConfigKey);

            DateTime dateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "South Africa Standard Time");

            if (settings != null)
            {
                settings.LastWaybillNumber = lastWaybillNumber;
                settings.ModifiedDate = dateTime;
                settings.ModifiedUser = modifiedUser;
                context.SaveChanges();
            }
        }
    }
}