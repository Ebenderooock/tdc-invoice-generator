using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InvoiceGenerator_Core.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
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


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Client> Clients{ get; set; }
        
        public DbSet<Transporter> Transporters { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public DbSet<ProductBranch> ProductBranches { get; set; }

        public DbSet<ClientBranch> ClientBranches { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InvoiceItem>().Property(x => x.TotalKg).HasPrecision(18, 3);
            modelBuilder.Entity<InvoiceItem>().Property(x => x.UnitSize).HasPrecision(18, 3);
        }
    }
}