using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TDC_Invoice_Generator.Models
{
    public class Branch
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
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


        public List<Product> Products { get; set; }

        public Branch()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}