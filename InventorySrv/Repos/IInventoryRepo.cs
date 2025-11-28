
using InventorySrv.Dtos;
using Shared.Models;

namespace InventorySrv.Repos
{
    public interface IInventoryRepo
    {
        void CreateInventory(InventoryItem Inventory);
        void UpdateInventory(InventoryItem Inventory);
        void DeleteInventory(Guid id);

        Task<IEnumerable<InventoryItem>> GetAllInventorysAsync();
        Task<IEnumerable<InventoryItem>> GetAllInventorysByUserAsync(int start, int end);
        Task<IEnumerable<InventoryItem>> GetAllInventorysByUserAsync(InventoryFilterDto filter, int start, int end);
        Task<int> GetAllInventorysNumberAsync(InventoryFilterDto filter);
        Task<InventoryItem?> GetInventoryAsync(Guid id);

        Task<bool> SaveChangesAsync();
    }
}