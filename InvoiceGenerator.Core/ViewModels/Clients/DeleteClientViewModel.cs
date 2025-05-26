using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InvoiceGenerator.Core.ViewModels.Clients
{
    public class DeleteClientViewModel
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
    }
}