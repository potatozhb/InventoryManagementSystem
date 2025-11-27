using InventoryManagementApp.Infras;
using InventoryManagementApp.Models;
using InventoryManagementApp.Services;
using InventoryManagementApp.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace InventoryManagementApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const int PageSize = 4;  // items per page

        private readonly IInventoryService _inventory;

        private ObservableCollection<InventoryItem> Items { get; } = new();
        private ICollectionView FilteredItems { get; set; }

        public SearchBarViewModel SearchBarVM { get; }
        public DataItemsViewModel DataItemsVM { get; }
        public EditBarViewModel EditBarVM { get; }

        private int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set 
            { 
                _currentPage = value; 
                OnPropertyChanged(nameof(CurrentPage)); 
               // RefreshPagedItems(); 
            }
        }

        public int TotalPages =>
            (int)Math.Ceiling((double)FilteredItems.Cast<object>().Count() / PageSize);


        public MainWindowViewModel(
            IServiceFactory inventory,
            IFeatureManagerService feature, 
            ISeedDataService seed,
            SearchBarViewModel searchBarVM,
            DataItemsViewModel dataItemsVM, 
            EditBarViewModel editBarVM)
        {
            if(!feature.IsUseSqlDb)
            {
                seed.Seed();
                _inventory = inventory.GetService(nameof(InMemoryInventoryService));
            }
            else
            {
                _inventory = inventory.GetService(nameof(ApiSqlInventoryService));
            }


            SearchBarVM = searchBarVM;
            DataItemsVM = dataItemsVM;
            EditBarVM = editBarVM;

            editBarVM.SetCallback(HandleInventoryOperation);
            searchBarVM.PropertyChanged += OnSearchCriteriaChanged;

            // load
            _ = Refresh();

        }

        private void OnSearchCriteriaChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBarVM.SearchName)
                || e.PropertyName == nameof(SearchBarVM.SearchCategory))
            {
                FilteredItems.Refresh();
            }
        }


        private bool FilterItems(object obj)
        {
            if (obj is not InventoryItem item) return false;

            // Filter by name
            bool nameMatch = string.IsNullOrWhiteSpace(SearchBarVM.SearchName)
                || item.Name.Contains(SearchBarVM.SearchName, StringComparison.OrdinalIgnoreCase);

            // Filter by category
            bool categoryMatch = SearchBarVM.SearchCategory == "All"
                || item.Category == SearchBarVM.SearchCategory;

            return nameMatch && categoryMatch;
        }

        public async Task Refresh()
        {
            var list = await _inventory.GetItemsAsync();
            Items.Clear();
            foreach (var i in list) Items.Add(i);

            FilteredItems = CollectionViewSource.GetDefaultView(Items);
            FilteredItems.Filter = FilterItems; 
            DataItemsVM.ItemsView = FilteredItems;
        }

        public async Task HandleInventoryOperation(Operations operation, InventoryItem item)
        {
            switch (operation)
            {
                case Operations.Add:
                    await _inventory.AddItemAsync(item);
                    break;
                case Operations.Edit:
                    await _inventory.UpdateItemAsync(item);
                    break;
                case Operations.Delete:
                    await _inventory.DeleteItemAsync(item.Id);
                    break;
            }

            await Refresh();//this is better efficient way to refresh for big data set
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
