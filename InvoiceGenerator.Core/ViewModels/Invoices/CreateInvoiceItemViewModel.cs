using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using InvoiceGenerator.Core.Models;

namespace InvoiceGenerator.Core.ViewModels.Invoices
{
    public class CreateInvoiceItemViewModel
    {
        [Display(Name = "Quantity")]
        public short? Quantity { get; set; }

        [Display(Name = "Unit Size")]
        public decimal? UnitSize { get; set; }

        [Display(Name = "Total Kg")]
        public decimal? TotalKg { get; set; }

        [Display(Name = "Pallets")]
        public string Pallets { get; set; }

        public string BatchNumbers { get; set; }

        public Product Product { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductId { get; set; }
    }
}