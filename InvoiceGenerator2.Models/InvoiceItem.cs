using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceGenerator2.Models
{
    public class InvoiceItem
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }

        public Invoice Invoice { get; set; }

        [Required]
        [StringLength(128)]
        public string InvoiceId { get; set; }

        [Display(Name = "Quantity")]
        public short? Quantity { get; set; }

        [Display(Name = "Unit Size")]
        public decimal? UnitSize { get; set; }

        [Display(Name = "Unit Size")]
        public decimal? TotalKg { get; set; }

        public Product Product { get; set; }

        [Required]
        [StringLength(128)]
        public string ProductId { get; set; }

        [Display(Name = "Pallets")]
        public string Pallets { get; set; }

        public string BatchNumbers { get; set; }

        public int Order { get; set; }

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

        public InvoiceItem()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}