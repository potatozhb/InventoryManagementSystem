using AutoMapper;
using InventorySrv.Dtos;
using InventorySrv.Models;
using Shared.Models;
using InventorySrv.Repos;

namespace InventorySrv.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepo _InventoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public InventoryService(IInventoryRepo InventoryRepo, IMapper mapper, ILogger<AuthService> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _InventoryRepo = InventoryRepo;
        }


        public async Task<PagedResultDto<InventoryItem>> GetInventorysAsync(InventoryFilterDto? filter, int? start, int? end)
        {
            IEnumerable<InventoryItem> Inventorys;

            int s = start != null ? start.Value : 0;
            int e = end != null ? end.Value : s + 20;
            Inventorys = await _InventoryRepo.GetAllInventorysByUserAsync(filter, s, e);

            var res = new PagedResultDto<InventoryItem>();

            if (!Inventorys.Any())
            {
                _logger.LogWarning($"--> Not found or has no Inventory data");
                res.Items = Enumerable.Empty<InventoryItem>();
            }
            res.TotalPages = Inventorys.ToList().Count / (e - s) +1;
            res.CurrentPage = s / (e - s) + 1;

            res.Items = Inventorys;
            return res;
        }

        public async Task<InventoryItem> CreateInventoryForUserAsync(InventoryItem inventory)
        {
            var Inventory = _mapper.Map<InventoryItem>(inventory);

            _InventoryRepo.CreateInventory(Inventory);
            await _InventoryRepo.SaveChangesAsync();

            return Inventory;
        }

        public async Task<InventoryItem> GetInventoryAsync(Guid id)
        {
            var inventory = await _InventoryRepo.GetInventoryAsync(id);
            return inventory;
        }

        public async Task<InventoryItem> UpdateInventoryForUserAsync(InventoryItem inventory)
        {
            _InventoryRepo.UpdateInventory(inventory);
            await _InventoryRepo.SaveChangesAsync();
            return inventory;
        }

        public async Task<InventoryItem> RemoveInventoryAsync(Guid id)
        {
            _InventoryRepo.DeleteInventory(id);
            await _InventoryRepo.SaveChangesAsync();
            return null;
        }
    }
}