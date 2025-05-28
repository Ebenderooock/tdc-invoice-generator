using InvoiceGenerator2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator2.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ProductBranch> ProductBranches { get; set; }
        public DbSet<ClientBranch> ClientBranches { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InvoiceItem>().Property(x => x.TotalKg).HasPrecision(18, 3);
            builder.Entity<InvoiceItem>().Property(x => x.UnitSize).HasPrecision(18, 3);
        }
    }
}