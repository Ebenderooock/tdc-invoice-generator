using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator2.Models.ViewModels.Users
{
    public class EditUserViewModel
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [StringLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [StringLength(75)]
        public string LastName { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> BranchCodes { get; set; } = new List<SelectListItem>
        {
                new SelectListItem {Text = "Johannesburg", Value = "JHB"},
                new SelectListItem {Text = "Cape Town", Value = "CPT"}
        };
        public List<Role> Roles { get; set; }
    }
}