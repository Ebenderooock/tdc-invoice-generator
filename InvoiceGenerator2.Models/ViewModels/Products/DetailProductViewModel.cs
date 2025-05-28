using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator2.Models.ViewModels.Products
{
    public class DetailProductViewModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Branch Name")]
        public string BranchName { get; set; }

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