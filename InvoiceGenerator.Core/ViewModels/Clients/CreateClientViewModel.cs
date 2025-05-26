using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TDC_Invoice_Generator.ViewModels.Clients
{
    public class CreateClientViewModel
    {
        [Required]
        [Display(Name = "Account Number")]
        [StringLength(75)]
        public string AccountNumber { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        [StringLength(75)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Client Address")]
        [StringLength(75)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "CP Name")]
        [StringLength(75)]
        public string ContactPersonName { get; set; }

        [Required]
        [Display(Name = "CP Phone Number")]
        [StringLength(10)]
        public string ContactPersonPhoneNumber { get; set; }

        [Display(Name = "Branches")]
        [MaxLength(500)]
        public List<SelectListItem> Branches { get; set; }

        [Display(Name = "SelectedBranches")]
        [MaxLength(500)]
        [MinLength(1, ErrorMessage = "At least 1 branch is required")]
        public IEnumerable<string> SelectedBranches { get; set; }
    }
}