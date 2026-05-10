using E_Ticaret.Models;
using Microsoft.EntityFrameworkCore;


namespace E_Ticaret.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        public DbSet<User> Users => Set<User>();

    }
}
