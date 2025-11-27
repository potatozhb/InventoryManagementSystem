using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.Infras
{
    public abstract class BaseViewModel : ObservableObject
    {
        // common properties and lifecycle methods
        public virtual void Initialize(object parameter) { }
    }
}
