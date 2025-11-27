using Shared.Models;

namespace InventoryManagementApp.Services.Interfaces
{
    using InventoryManagementApp.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public interface IInventoryService
    {
        Task<InventoryItem> AddItemAsync(InventoryItem item);
        Task<InventoryItem> UpdateItemAsync(InventoryItem item);
        Task DeleteItemAsync(Guid id);
        Task<InventoryItem> GetItemAsync(Guid id);
        Task<IEnumerable<InventoryItem>> GetItemsAsync(
            int? startIndex = null,
            int? endIndex = null,
            Filters? filter = null);
    }
}
