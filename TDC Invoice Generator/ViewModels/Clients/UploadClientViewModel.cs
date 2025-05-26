using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TDC_Invoice_Generator.ViewModels.Clients
{
    public class UploadClientViewModel
    {
        [Required]
        [Display(Name = "Excel File")]
        public HttpPostedFileBase File { get; set; }
    }
}