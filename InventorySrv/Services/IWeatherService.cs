using InventorySrv.Dtos;
using InventorySrv.Models;

namespace InventorySrv.Services
{
    public interface IInventoryService
    {
        Task<IEnumerable<Inventory>> GetInventorysAsync();
        Task<IEnumerable<InventoryReadDto>> GetInventorysAsync(string userId, int? start, int? end);
        Task<InventoryReadDto> CreateInventoryForUserAsync(string userId, InventoryCreateDto InventoryCreateDto);
    }
}