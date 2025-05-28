using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InvoiceGenerator2.Models.ViewModels.Products
{
    public class UploadProductViewModel
    {
        [Required]
        [Display(Name = "Excel File")]
        public IFormFile File { get; set; }
    }
}