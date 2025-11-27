using InventoryManagementApp.Models;
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

        public Task<IEnumerable<InventoryItem>> GetItemsAsync(
            int? startIndex = null, 
            int? endIndex = null, 
            Filters? filter = null)
        {
            var q = _store.Values.AsEnumerable();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchName))
                    q = q.Where(i => i.Name.Contains(filter.SearchName.Trim(), StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(filter.SearchCategory))
                    q = q.Where(i => i.Name.Contains(filter.SearchCategory.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            
            if (startIndex != null && endIndex != null)
                q = q.Skip(startIndex.Value).Take(endIndex.Value - startIndex.Value);
            return Task.FromResult(q);
        }


        public Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }
    }
}
