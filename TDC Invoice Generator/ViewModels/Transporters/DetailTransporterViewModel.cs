using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator_Core.ViewModels.Transporters
{
    public class DetailTransporterViewModel
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
        
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }
    }
}