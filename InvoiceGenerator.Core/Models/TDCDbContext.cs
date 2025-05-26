using System.Data.Entity;
using TDC_Invoice_Generator.ViewModels.Users;

namespace TDC_Invoice_Generator.Models
{
    public class TDCDbContext : DbContext
    {
        public TDCDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}