using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Invoices
{
    public class DeleteInvoiceViewModel
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

        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Display(Name = "Transporter")]
        public string Transporter { get; set; }

        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}