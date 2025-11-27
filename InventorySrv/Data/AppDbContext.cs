
using Microsoft.EntityFrameworkCore;
using InventorySrv.Models;

namespace InventorySrv.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Inventory> Inventorys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}