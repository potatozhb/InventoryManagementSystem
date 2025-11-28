
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using InventorySrv.Data;
using Shared.Models;
using InventorySrv.Dtos;

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

        public void CreateInventory(InventoryItem Inventory)
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

        public async Task<IEnumerable<InventoryItem>> GetAllInventorysAsync()
        {
            return await this._context.Inventorys.ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetAllInventorysByUserAsync(int start, int end)
        {
            return await this._context.Inventorys
                .Skip(start)
                .Take(end - start)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventoryItem>> GetAllInventorysByUserAsync(InventoryFilterDto filter, int start, int end)
        {
            if (start < 0) start = 0;
            if (end < start) end = start;

            int count = end - start;

            IQueryable<InventoryItem> query = _context.Inventorys;

            // Apply filters first
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    query = query.Where(i => i.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(filter.Category) && filter.Category != "All")
                {
                    query = query.Where(i =>
                                        i.Category != null &&
                                        i.Category.ToLower() == filter.Category.ToLower());
                }
            }
            // Then apply paging
            query = query.OrderBy(o => o.CreateTime).Skip(start).Take(count);

            return await query.ToListAsync();
        }

        public async Task<int> GetAllInventorysNumberAsync(InventoryFilterDto filter)
        {
            IQueryable<InventoryItem> query = _context.Inventorys;

            // Apply filters first
            if (filter != null)
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    query = query.Where(i => i.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(filter.Category) && filter.Category != "All")
                {
                    query = query.Where(i =>
                                        i.Category != null &&
                                        i.Category.ToLower() == filter.Category.ToLower());
                }
            }
            
            return query.ToListAsync().Result.Count;
        }

        public async Task<InventoryItem?> GetInventoryAsync(Guid id)
        {
            if (this._cache.TryGetValue(id, out var data) && data is InventoryItem Inventory)
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

        public void UpdateInventory(InventoryItem Inventory)
        {
            if (Inventory == null) throw new ArgumentNullException(nameof(Inventory));

            this._cache.Remove(Inventory.Id);
            var curInventory = this.GetInventoryAsync(Inventory.Id).Result;
            if (curInventory == null)
                throw new InvalidOperationException("Inventory not found");

            curInventory.Name = Inventory.Name;
            curInventory.Category = Inventory.Category;
            curInventory.Quantity = Inventory.Quantity;
            curInventory.Price = Inventory.Price;
        }
    }
}