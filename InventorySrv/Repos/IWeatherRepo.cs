
using InventorySrv.Models;

namespace InventorySrv.Repos
{
    public interface IInventoryRepo
    {
        void CreateInventory(Inventory Inventory);
        void UpdateInventory(Inventory Inventory);
        void DeleteInventory(Guid id);

        Task<IEnumerable<Inventory>> GetAllInventorysAsync();
        Task<IEnumerable<Inventory>> GetAllInventorysByUserAsync(string userId);
        Task<IEnumerable<Inventory>> GetAllInventorysByUserAsync(string userId, int start, int end);
        Task<Inventory?> GetInventoryAsync(Guid id);

        Task<bool> SaveChangesAsync();
    }
}