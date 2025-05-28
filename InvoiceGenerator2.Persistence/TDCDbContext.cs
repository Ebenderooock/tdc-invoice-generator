using InvoiceGenerator2.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator2.Persistence
{
    public class TDCDbContext : DbContext
    {
        public TDCDbContext(DbContextOptions<TDCDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}