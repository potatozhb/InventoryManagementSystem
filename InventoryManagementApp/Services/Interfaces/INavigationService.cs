using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InventoryManagementApp.Services.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(string view, object parameter = null);

        void Navigate<T>(Window currentWindow) where T : Window;
    }
}
