using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceGenerator.Core.Models
{
    public class Client
    {
        [Key]
        [Display(Name = "Id")]
        [StringLength(128)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [StringLength(75)]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        [StringLength(100)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "CP Name")]
        [StringLength(75)]
        public string ContactPersonName { get; set; }

        [Required]
        [Display(Name = "CP Phone Number")]
        [StringLength(20)]
        public string ContactPersonPhoneNumber { get; set; }

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
        public List<Invoice> Invoices { get; set; } 

        public List<ClientBranch> Branches { get; set; }

        public Client()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}