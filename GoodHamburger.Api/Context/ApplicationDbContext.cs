using GoodHamburger.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Api.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<MenuItem> MenuItens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}