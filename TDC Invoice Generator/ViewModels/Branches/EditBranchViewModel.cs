using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator_Core.ViewModels.Branches
{
    public class EditBranchViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Branch Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [StringLength(75)]
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
    }
}