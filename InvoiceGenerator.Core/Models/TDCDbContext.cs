using System.Data.Entity;
using InvoiceGenerator.Core.ViewModels.Users;

namespace InvoiceGenerator.Core.Models
{
    public class TDCDbContext : DbContext
    {
        public TDCDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}