using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator_Core.ViewModels.Transporters
{
    public class EditTransporterViewModel
    {
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }
        
        [Required]
        [Display(Name = "Transporter")]
        [StringLength(75)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Contact Person")]
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [Required]
        [Display(Name = "Contact Number")]
        [StringLength(20)]
        public string ContactNumber { get; set; }
    }
}