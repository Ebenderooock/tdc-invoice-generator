using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace InvoiceGenerator2.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter Name")]
        [StringLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Please enter Surname")]
        [StringLength(75)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Created User")]
        [StringLength(75)]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }

        public DateTime? DeletedDate { get; set; }

        [StringLength(75)]
        public string DeletedUser { get; set; }

        public bool IsDeleted { get; set; }

        public async Task<ClaimsPrincipal> GenerateUserPrincipalAsync(UserManager<ApplicationUser> manager)
        {
            var identity = new ClaimsIdentity(await manager.GetClaimsAsync(this), "Identity.Application");
            return new ClaimsPrincipal(identity);
        }
    }

}
