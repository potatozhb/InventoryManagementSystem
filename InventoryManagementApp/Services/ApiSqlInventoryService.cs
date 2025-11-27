using InventoryManagementApp.Infras;
using InventoryManagementApp.Models;
using InventoryManagementApp.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Shared.Models;

namespace InventoryManagementApp.Services
{
    public class ApiSqlInventoryService : IInventoryService
    {
        private readonly ConcurrentDictionary<Guid, InventoryItem> _store = new();
        private readonly HttpClient _http;

        public ApiSqlInventoryService(HttpClient http)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
        }

        public Task<InventoryItem> AddItemAsync(InventoryItem item)
        {
            _store[item.Id] = item;
            return Task.FromResult(item);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var response = await _http.DeleteAsync(string.Format(ApiEndpoints.DeleteItem, id));
            response.EnsureSuccessStatusCode();
        }

        public async Task<InventoryItem> GetItemAsync(Guid id)
        {
            var response = await _http.GetAsync(string.Format(ApiEndpoints.GetItemById, id));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InventoryItem>() ?? throw new Exception("Item not found");
        }

        public async Task<IEnumerable<InventoryItem>> GetItemsAsync(int? startIndex = null, int? endIndex = null, Filters? filter = null)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter?.SearchName))
                queryParams.Add($"name={Uri.EscapeDataString(filter.SearchName)}");

            if (!string.IsNullOrWhiteSpace(filter?.SearchCategory))
                queryParams.Add($"category={Uri.EscapeDataString(filter.SearchCategory)}");

            string queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            var response = await _http.GetAsync(string.Format(ApiEndpoints.GetItems, queryString));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<InventoryItem>>() ?? Array.Empty<InventoryItem>();
        }

        public async Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            if (item.Id == Guid.Empty)
                throw new ArgumentException("Item ID must be set for update.");

            var response = await _http.PutAsJsonAsync(ApiEndpoints.UpdateItem +  $"{item.Id}", item);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InventoryItem>() ?? item;
        }
    }
}
