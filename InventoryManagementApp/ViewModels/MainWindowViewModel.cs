using InventoryManagementApp.EventAggregator;
using InventoryManagementApp.Infras;
using InventoryManagementApp.Services;
using InventoryManagementApp.Services.Interfaces;
using Shared.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace InventoryManagementApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private const int PageSize = 4;  // items per page, can compute base on screen size, now just give it fixed number

        private readonly IInventoryService _inventory;

        private ObservableCollection<InventoryItem> Items { get; } = new();
        private ICollectionView FilteredItems { get; set; }

        public SearchBarViewModel SearchBarVM { get; }
        public DataItemsViewModel DataItemsVM { get; }
        public EditBarViewModel EditBarVM { get; }

        
        public int CurrentPage { get; set; } = 1;

        public int TotalPages { get; set; } = 1;


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

            Messenger.Instance.Subscribe<PageChangedMessage>(msg => OnPageChanged(msg.NewPage));
            Messenger.Instance.Subscribe<RequestPageInfoMessage>(_ => SendPageInfo());

            // load
            _ = Refresh(null, 0, PageSize);
        }

        private async void OnPageChanged(int newPage)
        {
            CurrentPage = newPage;
            await LoadPageAsync();
            SendPageInfo();
        }

        private void SendPageInfo()
        {
            Messenger.Instance.Publish(new ProvidePageInfoMessage
            {
                CurrentPage = CurrentPage,
                TotalPage = TotalPages
            });
        }

        private async Task LoadPageAsync()
        {
            int start = (CurrentPage-1) *PageSize;
            int end = CurrentPage * PageSize;
            var filter = new Filters()
            {
                SearchName = SearchBarVM.SearchName,
                SearchCategory = SearchBarVM.SearchCategory
            };
            Refresh(filter, start, end);
        }

        private void OnSearchCriteriaChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBarVM.SearchName)
                || e.PropertyName == nameof(SearchBarVM.SearchCategory))
            {
                //FilteredItems.Refresh();
                //filter from backend
                var filter = new Filters() { SearchName = SearchBarVM.SearchName
                    , SearchCategory = SearchBarVM.SearchCategory};

                Refresh(filter, 0, PageSize); 
            }
        }

        /// <summary>
        /// Filter on front end
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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

        public async Task Refresh(Filters? filter = null, int? start = null, int? end = null)
        {
            var res = await _inventory.GetItemsAsync(filter, start, end);
            CurrentPage = res.CurrentPage;
            TotalPages = res.TotalPages;
            var list = res.Items;
            Items.Clear();
            foreach (var i in list) Items.Add(i);

            FilteredItems = CollectionViewSource.GetDefaultView(Items);
            FilteredItems.Filter = FilterItems; 
            DataItemsVM.ItemsView = FilteredItems;
            SendPageInfo();
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

            await Refresh(null, 0, PageSize);//this is better efficient way to refresh for big data set
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
