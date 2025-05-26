using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;
using TDC_Invoice_Generator.Models;

namespace TDC_Invoice_Generator.ViewModels.Invoices
{
    public class EditInvoiceViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Client")]
        [StringLength(128)]
        public string ClientId { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "P/O Number")]
        [StringLength(75)]
        public string PoNumber { get; set; }

        [Display(Name = "Transporter P/O Number")]
        [StringLength(75)]
        public string TransporterPoNumber { get; set; }

        [Display(Name = "General Waybill Number")]
        [StringLength(75)]
        public string GeneralWaybillNumber { get; set; }

        [Required]
        [Display(Name = "From Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }

        [Required]
        [Display(Name = "Status")]
        [StringLength(10)]
        public string Status { get; set; }
        
        [Display(Name = "Transporter")]
        [StringLength(128)]
        public string TransporterId { get; set; }
        
        public bool SaveAndDownload { get; set; }

        public List<SelectListItem> StatusItems { get; } = new List<SelectListItem>
        {
                new SelectListItem {Text = "Pending", Value = "PD"},
                new SelectListItem {Text = "Delivered", Value = "DL"}
        };
        
        public IEnumerable<EditInvoiceViewModelBranchList> Branches { get; set; }

        public IEnumerable<EditInvoiceViewModelClientList> Clients { get; set; }
        
        public IEnumerable<EditInvoiceViewModelTransporterList> Transporters { get; set; }

        public List<EditInvoiceItemViewModel> OldInvoiceItems { get; set; }

        public List<EditInvoiceViewModelProductList> Products { get; set; }

        public List<CreateInvoiceItemViewModel> InvoiceItems { get; set; }

        public EditInvoiceViewModel()
        {
            InvoiceItems = new List<CreateInvoiceItemViewModel>();
        }

    }

    public class EditInvoiceViewModelBranchList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class EditInvoiceViewModelClientList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class EditInvoiceViewModelProductList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    
    public class EditInvoiceViewModelTransporterList
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}