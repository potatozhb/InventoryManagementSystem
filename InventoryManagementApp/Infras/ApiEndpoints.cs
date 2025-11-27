using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Infras
{
    public static class ApiEndpoints
    {
        public const string Base = "api/v1/inventory";

        public const string AddItem = Base;                  // POST v1/items
        public const string UpdateItem = Base + "/patch/{0}"; // PUT v1/patch/items/{id}
        public const string DeleteItem = Base + "/{0}";       // DELETE v1/items/{id}
        public const string GetItemById = Base + "/{0}";      // GET v1/items/{id}
        public const string GetItems = Base;                   // GET v1/items (with optional query)
    }
}
