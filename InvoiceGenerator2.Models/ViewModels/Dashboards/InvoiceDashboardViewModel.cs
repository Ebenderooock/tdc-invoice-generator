﻿namespace InvoiceGenerator2.Models.ViewModels.Dashboards
{
    public class InvoiceDashboardViewModel
    {
        public int TotalInvoices { get; set; }
        public int TotalProducts { get; set; }
        public int TotalClients { get; set; }
        public int TotalUsers { get; set; }
    }

    public class InvoiceDashboardViewModelChart
    {
        public string Month { get; set; }
        public int Invoices { get; set; }
    }
}