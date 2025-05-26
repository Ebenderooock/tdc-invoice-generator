using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceGenerator.Core.ViewModels.Clients
{
    public class DetailClientViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Client Name")]
        public string Name { get; set; }

        [Display(Name = "Client Address")]
        public string Address { get; set; }

        [Display(Name = "CP Name")]
        public string ContactPersonName { get; set; }

        [Display(Name = "CP Phone Number")]
        public string ContactPersonPhoneNumber { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }

        public List<string> Branches { get; set; }
    }
}