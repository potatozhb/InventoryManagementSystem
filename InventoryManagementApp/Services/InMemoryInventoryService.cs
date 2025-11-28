
using InventoryManagementApp.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using Shared.Models;

namespace InventoryManagementApp.Services
{
    public class InMemoryInventoryService : IInventoryService
    {
        private readonly ConcurrentDictionary<Guid, InventoryItem> _store = new();


        public Task<InventoryItem> AddItemAsync(InventoryItem item)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }


        public Task DeleteItemAsync(Guid id)
        {
            _store.Remove(id, out _);
            return Task.CompletedTask;
        }


        public Task<InventoryItem> GetItemAsync(Guid id)
        {
            _store.TryGetValue(id, out var item);
            return Task.FromResult(item);
        }

        public Task<PagedResultDto<InventoryItem>> GetItemsAsync(
            Filters? filter = null,
            int? startIndex = null, 
            int? endIndex = null)
        {
            var q = _store.Values.AsEnumerable();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchName))
                    q = q.Where(i => i.Name.Contains(filter.SearchName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(filter.SearchCategory) && filter.SearchCategory != "All")
                    q = q.Where(i => i.Category.Contains(filter.SearchCategory.Trim(), StringComparison.OrdinalIgnoreCase));
            }

            int s = startIndex != null ? startIndex.Value : 0;
            int e = endIndex != null ? endIndex.Value : s + 20;

            var res = new PagedResultDto<InventoryItem>();
            res.TotalPages = q.ToList().Count / (e-s) + 1;
            res.CurrentPage = s/(e-s) +1;
            q = q.OrderBy(o => o.CreateTime).Skip(s).Take(e - s);
            res.Items = q.ToList();
            
            return Task.FromResult(res);
        }


        public Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }
    }
}
