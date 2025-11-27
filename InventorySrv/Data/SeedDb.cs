using InventorySrv.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace InventorySrv.Data
{
    public static class SeedDb
    {
        public static void InitializeDb(IApplicationBuilder app, bool isDev)
        {
            using (var service = app.ApplicationServices.CreateScope())
            {
                Seed(service.ServiceProvider.GetService<AppDbContext>(), isDev);
            }
        }

        private static void Seed(AppDbContext context, bool isDev)
        {
            if (!isDev)
            {
                Console.WriteLine("--> Attempting to apply migration...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Inventorys.Any())
            {
                Console.WriteLine("--> Seeding data...");
                DateTime utc = DateTime.UtcNow.AddHours(-1);
                context.Inventorys.AddRange(
                    new InventoryItem { Name = "Laptop", Category = "Electronics", Quantity = 10 },
                    new InventoryItem { Name = "Smartphone", Category = "Electronics", Quantity = 25 },
                    new InventoryItem { Name = "Jeans", Category = "Clothing", Quantity = 50 },
                    new InventoryItem { Name = "Apple", Category = "Food", Quantity = 100 },
                    new InventoryItem { Name = "Banana", Category = "Food", Quantity = 150 }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Data already exists.");
            }

        }
    }
}