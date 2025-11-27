using InventoryManagementApp.Models;
using InventoryManagementApp.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Services
{
    public class ApiSqlInventoryService : IInventoryService
    {
        private readonly ConcurrentDictionary<Guid, InventoryItem> _store = new();
        private readonly HttpClient _http;

        public ApiSqlInventoryService(HttpClient http)
        {
            _http = http;
        }

        public Task<InventoryItem> AddItemAsync(InventoryItem item)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }

        public Task DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryItem> GetItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InventoryItem>> GetItemsAsync(int? startIndex = null, int? endIndex = null, Filters? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            throw new NotImplementedException();
        }
    }
}
