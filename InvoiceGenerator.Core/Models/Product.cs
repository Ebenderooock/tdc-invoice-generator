using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator.Core.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Product Code")]
        [StringLength(75)]
        public string Code { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Required]
        [Display(Name = "Created User")]
        [StringLength(75)]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        [StringLength(75)]
        public string ModifiedUser { get; set; }

        public DateTime? DeletedDate { get; set; }

        [StringLength(75)]
        public string DeletedUser { get; set; }

        public bool IsDeleted { get; set; }

        public string BranchId { get; set; }
        

        [ForeignKey("BranchId")]
        public Branch Branch { get; set; } = null;

        public List<ProductBranch> Branches { get; set; }

        public Product()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}