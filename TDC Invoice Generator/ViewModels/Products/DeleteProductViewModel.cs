using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TDC_Invoice_Generator.ViewModels.Products
{
    public class DeleteProductViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }
    }
}