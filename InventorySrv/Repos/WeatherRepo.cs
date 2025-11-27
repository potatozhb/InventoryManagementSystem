
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using InventorySrv.Data;
using InventorySrv.Models;

namespace InventorySrv.Repos
{
    public class InventoryRepo : IInventoryRepo
    {
        private const int CacheTTLMin = 5;

        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<InventoryRepo> _logger;
        public InventoryRepo(AppDbContext context, IMemoryCache cache, ILogger<InventoryRepo> logger)
        {
            this._context = context;
            this._cache = cache;
            this._logger = logger;
        }

        public void CreateInventory(Inventory Inventory)
        {
            if (Inventory == null) throw new ArgumentNullException(nameof(Inventory));
            this._context.Inventorys.Add(Inventory);
            this._cache.Set(Inventory.Id, Inventory, TimeSpan.FromMinutes(CacheTTLMin));
            this._logger.LogInformation($"--> Add new data");
        }

        public void DeleteInventory(Guid id)
        {
            var Inventory = GetInventoryAsync(id).Result;
            if (Inventory != null)
            {
                this._cache.Remove(Inventory.Id);
                this._context.Inventorys.Remove(Inventory);

                this._logger.LogInformation($"--> Remove a data");
            }
        }

        public async Task<IEnumerable<Inventory>> GetAllInventorysAsync()
        {
            return await this._context.Inventorys.ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetAllInventorysByUserAsync(string userId)
        {
            return await this._context.Inventorys.Where(w => w.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Inventory>> GetAllInventorysByUserAsync(string userId, int start, int end)
        {
            return await this._context.Inventorys
                .Where(w => w.UserId == userId)
                .OrderBy(w => w.UpdateTime)
                .Skip(start)
                .Take(end - start)
                .ToListAsync();
        }

        public async Task<Inventory?> GetInventoryAsync(Guid id)
        {
            if (this._cache.TryGetValue(id, out var data) && data is Inventory Inventory)
            {
                this._logger.LogInformation($"--> Get a data from cache");
                return Inventory;
            }

            Inventory = await _context.Inventorys.FirstOrDefaultAsync(w => w.Id == id);
            if (Inventory != null) this._cache.Set(id, Inventory, TimeSpan.FromMinutes(CacheTTLMin));

            this._logger.LogInformation($"--> Get a data from db");
            return Inventory;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await this._context.SaveChangesAsync() >= 0;
        }

        public void UpdateInventory(Inventory Inventory)
        {
            if (Inventory == null) throw new ArgumentNullException(nameof(Inventory));

            var curInventory = this.GetInventoryAsync(Inventory.Id).Result;
            if (curInventory == null)
                throw new InvalidOperationException("Inventory not found");

            curInventory.Rain = Inventory.Rain;
            curInventory.UserId = Inventory.UserId;
            curInventory.UpdateTime = Inventory.UpdateTime;
        }
    }
}