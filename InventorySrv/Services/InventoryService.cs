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

        public async Task<IEnumerable<InventoryItem>> GetInventorysAsync()
        {
            _logger.LogInformation("--> Getting Inventorys....");

            var Inventorys = await this._InventoryRepo.GetAllInventorysAsync();

            return Inventorys;
        }

        public async Task<IEnumerable<InventoryReadDto>> GetInventorysAsync(int? start, int? end)
        {
            IEnumerable<InventoryItem> Inventorys;

            if (start.HasValue && end.HasValue)
            {
                if (start < 0 || end <= start)
                {
                    _logger.LogError("--> Invalid paging parameters");
                    throw new ArgumentException("Invalid paging parameters");
                }

                Inventorys = await _InventoryRepo.GetAllInventorysByUserAsync(start.Value, end.Value);
            }
            else
            {
                Inventorys = await _InventoryRepo.GetAllInventorysAsync();
            }

            if (!Inventorys.Any())
            {
                _logger.LogWarning($"--> Not found or has no Inventory data");
                return Enumerable.Empty<InventoryReadDto>();
            }

            return _mapper.Map<IEnumerable<InventoryReadDto>>(Inventorys);
        }

        public async Task<InventoryReadDto> CreateInventoryForUserAsync(InventoryCreateDto InventoryCreateDto)
        {
            var Inventory = _mapper.Map<InventoryItem>(InventoryCreateDto);

            _InventoryRepo.CreateInventory(Inventory);
            await _InventoryRepo.SaveChangesAsync();

            return _mapper.Map<InventoryReadDto>(Inventory);
        }

    }
}