using Microsoft.EntityFrameworkCore;
using InventorySrv.Models;

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
                    new Inventory { UserId = "Bob", Rain = false, CreateTime = utc, UpdateTime = utc },
                    new Inventory { UserId = "Bob", Rain = true },
                    new Inventory { UserId = "Alice", Rain = true }
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