using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TDC_Invoice_Generator.Models;
using TDC_Invoice_Generator.ViewModels.Dashboards;

namespace TDC_Invoice_Generator.Controllers
{
    [Authorize(Roles = "" + RoleName.Admin + ", " + RoleName.Invoicing + "")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.ThirdParty))
            {
                return RedirectToAction("Index", "Invoices");
            }
            
            int totalInvoices = _context.Invoices.Where(i => i.IsDeleted == false).Count();
            int totalProducts = _context.Products.Where(i => i.IsDeleted == false).Count();
            int totalClients = _context.Clients.Where(i => i.IsDeleted == false).Count();
            int totalUsers = _context.Users.Where(i => i.IsDeleted == false).Count();

            InvoiceDashboardViewModel viewModel = new InvoiceDashboardViewModel
            {
                TotalInvoices = totalInvoices,
                TotalProducts = totalProducts,
                TotalClients = totalClients,
                TotalUsers = totalUsers,
            };

            return View(viewModel);
        }

        public JsonResult GetInvoicesPeMonth()
        {
            ArrayList totalInvoices = new ArrayList();
            if (totalInvoices == null)
            {
                throw new ArgumentNullException(nameof(totalInvoices));
            }

            ArrayList monthsData = new ArrayList();

            List<InvoiceDashboardViewModelChart> dataArray = _context.Database.SqlQuery<InvoiceDashboardViewModelChart>("SELECT CONCAT(DATENAME(MONTH,InvoiceDate), ' ' ,DATENAME(yyyy, InvoiceDate)) AS Month, COUNT(InvoiceDate) AS Invoices FROM Invoices WITH (NOLOCK) WHERE IsDeleted = 0 GROUP BY YEAR(InvoiceDate), MONTH(InvoiceDate),CONCAT(DATENAME(MONTH,InvoiceDate), ' ' , DATENAME(yyyy, InvoiceDate)) ORDER BY YEAR(InvoiceDate)").ToList();

            foreach (InvoiceDashboardViewModelChart item in dataArray)
            {
                monthsData.Add(item.Month);
            }

            foreach (InvoiceDashboardViewModelChart item in dataArray)
            {
                totalInvoices.Add(item.Invoices);
            }

            return Json(new { Months = monthsData, Invoices = totalInvoices }, JsonRequestBehavior.AllowGet);
        }
    }
}