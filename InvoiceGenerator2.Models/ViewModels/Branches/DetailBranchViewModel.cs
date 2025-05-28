using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Branches
{
    public class DetailBranchViewModel
    {
        [Key]
        public string Id { get; set; }
        
        [Display(Name = "Branch Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        
        [Display(Name = "Contact Person")]
        [StringLength(100)]
        public string ContactPerson { get; set; }
        
        [Display(Name = "Contact Number")]
        [StringLength(20)]
        public string ContactNumber { get; set; }
        
        [Display(Name = "Address")]
        [StringLength(200)]
        public string Address { get; set; }

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