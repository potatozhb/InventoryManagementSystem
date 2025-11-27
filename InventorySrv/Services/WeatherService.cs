using AutoMapper;
using InventorySrv.Dtos;
using InventorySrv.Models;
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

        public async Task<IEnumerable<Inventory>> GetInventorysAsync()
        {
            _logger.LogInformation("--> Getting Inventorys....");

            var Inventorys = await this._InventoryRepo.GetAllInventorysAsync();

            return Inventorys;
        }

        public async Task<IEnumerable<InventoryReadDto>> GetInventorysAsync(string userId, int? start, int? end)
        {
            if (string.IsNullOrWhiteSpace(userId) || userId.Length > 50)
            {
                _logger.LogError("--> userId can't be empty");
                throw new ArgumentException("Invalid userId");
            }

            IEnumerable<Inventory> Inventorys;

            if (start.HasValue && end.HasValue)
            {
                if (start < 0 || end <= start)
                {
                    _logger.LogError("--> Invalid paging parameters");
                    throw new ArgumentException("Invalid paging parameters");
                }

                _logger.LogInformation($"--> Getting Inventorys for user {userId} from index {start.Value} to {end.Value}....");
                Inventorys = await _InventoryRepo.GetAllInventorysByUserAsync(userId, start.Value, end.Value);
            }
            else
            {
                _logger.LogInformation($"--> Getting Inventorys for user {userId}....");
                Inventorys = await _InventoryRepo.GetAllInventorysByUserAsync(userId);
            }

            if (!Inventorys.Any())
            {
                _logger.LogWarning($"--> User {userId} not found or has no Inventory data");
                return Enumerable.Empty<InventoryReadDto>();
            }

            return _mapper.Map<IEnumerable<InventoryReadDto>>(Inventorys);
        }

        public async Task<InventoryReadDto> CreateInventoryForUserAsync(string userId, InventoryCreateDto InventoryCreateDto)
        {
            if (string.IsNullOrWhiteSpace(userId) || userId.Length > 50)
            {
                _logger.LogError("--> userId can't be empty");
                throw new ArgumentException("Invalid userId");
            }

            _logger.LogInformation($"--> Creating Inventorys for user {userId}....");

            var Inventory = _mapper.Map<Inventory>(InventoryCreateDto);
            Inventory.UserId = userId;

            _InventoryRepo.CreateInventory(Inventory);
            await _InventoryRepo.SaveChangesAsync();

            return _mapper.Map<InventoryReadDto>(Inventory);
        }

    }
}