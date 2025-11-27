using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Services.Interfaces
{
    public interface IServiceFactory
    {
        IInventoryService GetService(string key);
    }
}
