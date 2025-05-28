using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceGenerator2.Models
{
    [Table("AspNetUsers")]
    public class Users
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter Name")]
        [StringLength(75)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(75)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Email Confirmed")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Branch")]
        [StringLength(3)]
        public string BranchCode { get; set; }

        [Display(Name = "Phone Number Confirmed")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "Two Factor Enabled")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name = "Lockout End Date")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name = "Lockout Enabled")]
        public bool LockoutEnabled { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Access Failed Count")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "Created User")]
        public string CreatedUser { get; set; }

        [Display(Name = "Modified User")]
        public string ModifiedUser { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        [StringLength(75)]
        public string DeletedUser { get; set; }

        public bool IsDeleted { get; set; }

        public List<Role> Roles { get; set; }

    }

    // Gets all the roles and marks the ones assigned to user
    public class Role
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }

    // Gets the roles assigned to the user
    public class RoleList
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    } 
}