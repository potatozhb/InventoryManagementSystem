using InventoryManagementApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _serviceMap = new()
            {
                { nameof(InMemoryInventoryService), typeof(InMemoryInventoryService) },
                { nameof(ApiSqlInventoryService), typeof(ApiSqlInventoryService) }
            };

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IInventoryService GetService(string key)
        {
            if (_serviceMap.TryGetValue(key, out var type))
            {
                return (IInventoryService)_serviceProvider.GetRequiredService(type);
            }

            throw new KeyNotFoundException($"Service with key '{key}' not found.");
        }
    }
}
