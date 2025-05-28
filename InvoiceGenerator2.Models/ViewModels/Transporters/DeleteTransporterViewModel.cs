using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Transporters
{
    public class DeleteTransporterViewModel
    {
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }
        
        [Display(Name = "Transporter")]
        [StringLength(75)]
        public string Name { get; set; }
        
        [Display(Name = "Contact Person")]
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [Display(Name = "Contact Number")]
        [StringLength(20)]
        public string ContactNumber { get; set; }
    }
}