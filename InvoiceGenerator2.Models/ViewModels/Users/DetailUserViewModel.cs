using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InvoiceGenerator2.Models.ViewModels.Users
{
    public class DetailUserViewModel
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

        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }
        public List<Role> Roles { get; set; }
    }
}