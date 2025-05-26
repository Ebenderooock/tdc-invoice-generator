using System.Data.Entity;
using InvoiceGenerator_Core.ViewModels.Users;

namespace InvoiceGenerator_Core.Models
{
    public class TDCDbContext : DbContext
    {
        public TDCDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}