using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Invoices
{
    public class EditInvoiceItemViewModel
    {
        [Required]
        [Display(Name = "Quantity")]
        public short? Quantity { get; set; }

        [Required]
        [Display(Name = "Unit Size")]
        public decimal? UnitSize { get; set; }

        [Required]
        [Display(Name = "Unit Size")]
        public decimal? TotalKg { get; set; }

        [Required]
        [Display(Name = "Pallets")]
        public string Pallets { get; set; }

        [Required]
        public string BatchNumbers { get; set; }

        public Product Product { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductName{ get; set; }
    }
}