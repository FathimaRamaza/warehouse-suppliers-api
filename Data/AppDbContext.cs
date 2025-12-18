using Microsoft.EntityFrameworkCore;
using Suppliers.API.Models;

namespace Suppliers.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
    }
}
