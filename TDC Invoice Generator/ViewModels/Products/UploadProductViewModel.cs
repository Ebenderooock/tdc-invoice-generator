using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TDC_Invoice_Generator.ViewModels.Products
{
    public class UploadProductViewModel
    {
        [Required]
        [Display(Name = "Excel File")]
        public HttpPostedFileBase File { get; set; }
    }
}