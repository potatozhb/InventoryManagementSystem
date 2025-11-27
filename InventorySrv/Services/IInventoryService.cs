using InventorySrv.Dtos;
using InventorySrv.Models;
using Shared.Models;

namespace InventorySrv.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<InventoryItem>> GetInventorysAsync();
        Task<IEnumerable<InventoryReadDto>> GetInventorysAsync(int? start, int? end);
        Task<InventoryReadDto> CreateInventoryForUserAsync(InventoryCreateDto InventoryCreateDto);
    }
}