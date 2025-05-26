using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TDC_Invoice_Generator.Models;

namespace TDC_Invoice_Generator.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required]
        [Display(Name = "Product Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(75)]
        public string Name { get; set; }



        [Display(Name = "Branches")]
        [MaxLength(500)]
        public List<SelectListItem> Branches { get; set; }

        [Display(Name = "SelectedBranches")]
        [MaxLength(500)]
        [MinLength(1, ErrorMessage = "At least 1 branch is required")]
        public IEnumerable<string> SelectedBranches { get; set; }
    }
}