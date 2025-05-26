using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InvoiceGenerator.Core.Models;

namespace InvoiceGenerator.Core.ViewModels.Invoices
{
    public class DetailInvoiceViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Client")]
        public string Client { get; set; }

        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "P/O Number")]
        public string PoNumber { get; set; }

        [Display(Name = "General Waybill Number")]
        public string GeneralWaybillNumber { get; set; }

        [Display(Name = "From Branch")]
        public string BranchCode { get; set; }
        
        [Display(Name = "From Branch")]
        public string BranchName { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Display(Name = "Transporter")]
        public string Transporter { get; set; }
        
        [Display(Name = "Transporter P/O Number")]
        public string TransporterPoNumber { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }
        
        public bool ShouldDownload { get; set; }
    }
}