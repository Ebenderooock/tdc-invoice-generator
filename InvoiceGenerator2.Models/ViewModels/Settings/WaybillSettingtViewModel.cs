using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Settings
{
    public class WaybillSettingtViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Last Waybill Number")]
        public int LastWaybillNumber { get; set; }
    }
}