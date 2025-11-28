using Shared.Models;

namespace InventoryManagementApp.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<InventoryItem> AddItemAsync(InventoryItem item);
        Task<InventoryItem> UpdateItemAsync(InventoryItem item);
        Task DeleteItemAsync(Guid id);
        Task<InventoryItem> GetItemAsync(Guid id);
        Task<PagedResultDto<InventoryItem>> GetItemsAsync(
            Filters? filter = null,
            int? startIndex = null,
            int? endIndex = null);
    }
}
