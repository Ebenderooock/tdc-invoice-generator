﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceGenerator.Core.ViewModels.Users
{
    public class CreateUserViewModel
    {
        //[Key]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter Name")]
        [StringLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter Surname")]
        [StringLength(75)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }

        public List<SelectListItem> BranchCodes { get; set; } = new List<SelectListItem>
        {
                new SelectListItem {Text = "Johannesburg", Value = "JHB"},
                new SelectListItem {Text = "Cape Town", Value = "CPT"}
        };

        [Display(Name = "Phone Number")]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}