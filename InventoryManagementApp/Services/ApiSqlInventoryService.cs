using InventoryManagementApp.Infras;
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

        public async Task<InventoryItem> AddItemAsync(InventoryItem item)
        {
            var response = await _http.PostAsJsonAsync(ApiEndpoints.AddItem, item);
            response.EnsureSuccessStatusCode();
            return item;
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
            var res = await response.Content.ReadFromJsonAsync<InventoryItem>();
            return res ?? throw new Exception("Item not found");
        }

        public async Task<PagedResultDto<InventoryItem>> GetItemsAsync(
            Filters? filter = null,
            int? startIndex = null,
            int? endIndex = null)
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(filter?.SearchName))
                queryParams.Add($"name={Uri.EscapeDataString(filter.SearchName)}");

            if (!string.IsNullOrWhiteSpace(filter?.SearchCategory))
                queryParams.Add($"category={Uri.EscapeDataString(filter.SearchCategory)}");

            if (startIndex != null)
                queryParams.Add($"start={startIndex.Value}");
            if (endIndex != null)
                queryParams.Add($"end={endIndex.Value}");

            string queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";

            var response = await _http.GetAsync(string.Format(ApiEndpoints.GetItems, queryString));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PagedResultDto<InventoryItem>>() ?? new PagedResultDto<InventoryItem>();
        }

        public async Task<InventoryItem> UpdateItemAsync(InventoryItem item)
        {
            if (item.Id == Guid.Empty)
                throw new ArgumentException("Item ID must be set for update.");

            var response = await _http.PutAsJsonAsync(string.Format(ApiEndpoints.GetItems, item.Id), item);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<InventoryItem>() ?? item;
        }
    }
}
