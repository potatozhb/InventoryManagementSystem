using InventorySrv.Dtos;
using InventorySrv.Models;
using Shared.Models;

namespace InventorySrv.Services
{
    public interface IInventoryService
    {
        Task<InventoryItem> GetInventoryAsync(Guid id);
        Task<PagedResultDto<InventoryItem>> GetInventorysAsync(
            InventoryFilterDto? filter = null, int? start = null, int? end = null);
        Task<InventoryItem> CreateInventoryForUserAsync(InventoryItem inventory);
        Task<InventoryItem> UpdateInventoryForUserAsync(InventoryItem inventory);
        Task<InventoryItem> RemoveInventoryAsync(Guid id);
    }
}