using InventoryManagementApp.Infras;
using InventoryManagementApp.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace InventoryManagementApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Func<Window> _mainWindowFactory;


        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public void ShowWindow<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();
        }

        public void NavigateTo(string view, object parameter = null)
        {
            // Resolve view from DI
            var viewType = Type.GetType($"MyApp.App.Views.{view}View");
            var resolvedView = _serviceProvider.GetService(viewType) as Window;
            if (resolvedView == null) return;


            // Initialize ViewModel if supports Initialize
            if (resolvedView.DataContext is BaseViewModel vm)
                vm.Initialize(parameter);


            // Show window
            resolvedView.Show();
        }


        public void Navigate<T>(Window currentWindow) where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();
            currentWindow?.Close();
        }
    }
}
