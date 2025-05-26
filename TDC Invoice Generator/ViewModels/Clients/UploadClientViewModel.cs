using System.ComponentModel.DataAnnotations;
using System.Web;

namespace InvoiceGenerator_Core.ViewModels.Clients
{
    public class UploadClientViewModel
    {
        [Required]
        [Display(Name = "Excel File")]
        public HttpPostedFileBase File { get; set; }
    }
}