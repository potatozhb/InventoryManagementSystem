using InventoryManagementApp.EventAggregator;
using InventoryManagementApp.Infras;
using Shared.Models;
using System.ComponentModel;
using System.Windows;

namespace InventoryManagementApp.ViewModels
{
    public class DataItemsViewModel : INotifyPropertyChanged
    {
        public RelayCommandAsync NextCommand { get; }
        public RelayCommandAsync PreviousCommand { get; }

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage <= _totalPages && _currentPage != value)
                {
                    _currentPage = value;

                    OnPropertyChanged(nameof(CurrentPage)); 
                    NextCommand.RaiseCanExecuteChanged();
                    PreviousCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private int _totalPages = 5;
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                if (_totalPages != value)
                {
                    _totalPages = value;
                    OnPropertyChanged(nameof(TotalPages));
                }
            }
        }


        public DataItemsViewModel() 
        {
            Messenger.Instance.Subscribe<ProvidePageInfoMessage>(UpdatePageInfo);
            NextCommand = new RelayCommandAsync(OnNext, () => CurrentPage < TotalPages);
            PreviousCommand = new RelayCommandAsync(OnPrevious, () => CurrentPage > 1);
        }

        private void UpdatePageInfo(ProvidePageInfoMessage msg)
        {
            CurrentPage = msg.CurrentPage;
            TotalPages = msg.TotalPage;
        }

        private async Task OnPrevious()
        {
            if (_currentPage > 1)
                Messenger.Instance.Publish(new PageChangedMessage { NewPage = _currentPage - 1 });
        }

        private async Task OnNext()
        {
            if (_currentPage < _totalPages)
                Messenger.Instance.Publish(new PageChangedMessage { NewPage = _currentPage + 1 });
        }

        private ICollectionView _itemsView;

        public ICollectionView ItemsView
        {
            get => _itemsView;
            set { _itemsView = value; OnPropertyChanged(nameof(ItemsView)); }
        }

        private InventoryItem? _selectedItem;
        public InventoryItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));

                    Messenger.Instance.Publish(new SelectedItemChangedMessage(_selectedItem));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
