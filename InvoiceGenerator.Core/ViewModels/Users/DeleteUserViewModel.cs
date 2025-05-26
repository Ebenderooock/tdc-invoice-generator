using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceGenerator.Core.ViewModels.Users
{
    public class DeleteUserViewModel
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Branch")]
        public string BranchCode { get; set; }

        public List<SelectListItem> BranchCodes { get; } = new List<SelectListItem>
        {
                new SelectListItem {Text = "Johannesburg", Value = "JHB"},
                new SelectListItem {Text = "Cape Town", Value = "CPT"}
        };
    }
}