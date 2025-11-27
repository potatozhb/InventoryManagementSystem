using InventoryManagementApp.EventAggregator;
using InventoryManagementApp.Infras;
using InventoryManagementApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace InventoryManagementApp.ViewModels
{
    public class DataItemsViewModel : INotifyPropertyChanged
    {
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
