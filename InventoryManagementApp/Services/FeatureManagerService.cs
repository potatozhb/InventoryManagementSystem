using InventoryManagementApp.Services.Interfaces;
using System.Configuration;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Services
{
    public class FeatureManagerService : IFeatureManagerService
    {
        public bool IsUseSqlDb
        {
            get
            {
                var val = ConfigurationManager.AppSettings["UseSqlDb"];
                return bool.TryParse(val, out bool result) && result;
            }
        }

        public string ServiceUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ServiceUrl"];
            }

        }
    }
}
