
using InventoryManagementApp.Services.Interfaces;
using Shared.Models;

namespace InventoryManagementApp.Services
{
    public class SeedDataService : ISeedDataService
    {
        private readonly IInventoryService _inventoryService;
        public SeedDataService(IServiceFactory factory, IFeatureManagerService feature)
        {
            if(!feature.IsUseSqlDb)
                _inventoryService = factory.GetService(nameof(InMemoryInventoryService));
        }

        public void Seed()
        {
            var items = GetInitialInventory();
            foreach (var item in items) 
            {
                _inventoryService.AddItemAsync(item);
            }
        }

        private IEnumerable<InventoryItem> GetInitialInventory()
        {
            return new List<InventoryItem>
                {
                    new InventoryItem { Name = "Laptop", Category = "Electronics", Brand = "Brand1", Quantity = 10 },
                    new InventoryItem { Name = "Smartphone", Category = "Electronics", Brand = "Brand1", Quantity = 25 },
                    new InventoryItem { Name = "Jeans", Category = "Clothing", Brand = "Brand2", Quantity = 50 },
                    new InventoryItem { Name = "Apple", Category = "Food", Brand = "Brand1", Quantity = 100 },
                    new InventoryItem { Name = "Banana", Category = "Food", Brand = "Brand3", Quantity = 150 },
                    
                    new InventoryItem { Name = "Laptop", Category = "Electronics", Brand = "Brand4", Quantity = 10 },
                    new InventoryItem { Name = "Smartphone", Category = "Electronics", Brand = "Brand1", Quantity = 25 },
                    new InventoryItem { Name = "Jeans", Category = "Clothing", Brand = "Brand1", Quantity = 50 },
                    new InventoryItem { Name = "Apple", Category = "Food", Brand = "Brand1", Quantity = 100 },
                    new InventoryItem { Name = "Banana", Category = "Food", Brand = "Brand2", Quantity = 150 }
                };
        }

    }
}
