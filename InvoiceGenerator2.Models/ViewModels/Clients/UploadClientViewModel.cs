using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace InvoiceGenerator2.Models.ViewModels.Clients
{
    public class UploadClientViewModel
    {
        [Required]
        [Display(Name = "Excel File")]
        public IFormFile File { get; set; }
    }
}