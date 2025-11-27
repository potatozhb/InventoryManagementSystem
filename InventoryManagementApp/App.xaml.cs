using InventoryManagementApp.Infras;
using InventoryManagementApp.Models;
using InventoryManagementApp.Services;
using InventoryManagementApp.Services.Interfaces;
using InventoryManagementApp.ViewModels;
using InventoryManagementApp.Views.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.IO;
using System.Windows;

namespace InventoryManagementApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();

            var main = Services.GetRequiredService<MainWindow>();
            main.Show();
        }


        private void ConfigureServices(IServiceCollection services)
        {
            // Services
            services.AddSingleton<InMemoryInventoryService>();
            services.AddSingleton<IServiceFactory, ServiceFactory>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IFeatureManagerService, FeatureManagerService>();
            services.AddTransient<ISeedDataService, SeedDataService>();

            services.AddSingleton(typeof(Lazy<>), typeof(LazyResolver<>));

            services.AddHttpClient<ApiSqlInventoryService>((sp, client) =>
            {
                var featureManager = sp.GetRequiredService<IFeatureManagerService>();
                var apiUrl = featureManager.ServiceUrl;
                client.BaseAddress = new Uri(apiUrl);
            });

            // ViewModels
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<SearchBarViewModel>();
            services.AddTransient<DataItemsViewModel>();
            services.AddTransient<EditBarViewModel>();


            // Views
            services.AddTransient<MainWindow>(sp =>
                new MainWindow(sp.GetRequiredService<MainWindowViewModel>()));
            services.AddTransient<SearchBar>();
            services.AddTransient<DataItems>();
            services.AddTransient<EditBar>();


            // other registrations (logging, config, etc.)

        }
    }

}
