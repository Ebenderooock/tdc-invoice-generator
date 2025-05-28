using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator2.Models
{
    public class ProductBranch
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
        [Required]
        public string Id { get; set; }

        [ForeignKey("Product")]
        [Display(Name = "ProductId")]
        [StringLength(128)]
        [Required]
        public string ProductId { get; set; }

        [ForeignKey("Branch")]
        [Display(Name = "BranchId")]
        [StringLength(128)]
        [Required]
        public string BranchId { get; set; }


        public Branch Branch { get; set; }
        public Product Product { get; set; }
    }
}