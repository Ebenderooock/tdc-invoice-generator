using System;
using System.ComponentModel.DataAnnotations;

namespace InvoiceGenerator.Core.Models
{
    public class Transporter
    {
        [Key]
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

        public Transporter()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}