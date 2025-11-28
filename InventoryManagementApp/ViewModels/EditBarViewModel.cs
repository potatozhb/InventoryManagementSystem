using InventoryManagementApp.EventAggregator;
using InventoryManagementApp.Infras;
using Shared.Models;
using InventoryManagementApp.Services;
using InventoryManagementApp.Services.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Shared.Models;
using System.Windows.Input;

namespace InventoryManagementApp.ViewModels
{
    public class EditBarViewModel : INotifyPropertyChanged
    {
        private Func<Operations, InventoryItem, Task> _itemActionCallback;
        private readonly IInventoryService _inventoryService;
        private bool _canAdd = false;
        private bool _canEdit = false;
        private bool _canDelete = false;

        public EditBarViewModel(
            IFeatureManagerService feature, 
            IServiceFactory inventory)
        {
            if (!feature.IsUseSqlDb)
            {
                _inventoryService = inventory.GetService(nameof(InMemoryInventoryService));
            }
            else
            {
                _inventoryService = inventory.GetService(nameof(ApiSqlInventoryService));
            }

            AddCommand = new RelayCommandAsync(ExecuteAddAsync, () => _canAdd);
            EditCommand = new RelayCommandAsync(ExecuteEditAsync, () => _canEdit);
            DeleteCommand = new RelayCommandAsync(ExecuteDeleteAsync, () => _canDelete);
            Messenger.Instance.Subscribe<SelectedItemChangedMessage>(msg =>
            {
                CurrentItem = msg.SelectedItem;
            });
        }

        public void SetCallback(Func<Operations, InventoryItem, Task> callback)
        {
            _itemActionCallback = callback;
        }

        private InventoryItem? _currentItem;
        public InventoryItem? CurrentItem
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                OnPropertyChanged(nameof(CurrentItem));

                // Optionally update editing fields here
                if (_currentItem != null)
                {
                    ID = _currentItem.Id;
                    EditName = _currentItem.Name;
                    EditCategory = _currentItem.Category;
                    EditQuantity = _currentItem.Quantity.ToString();
                }
            }
        }

        private Guid? _id = null;
        public Guid? ID
        {
            get => _id;
            set 
            { 
                _id = value; 
                OnPropertyChanged(nameof(ID));

                CanAction();
            }
        }

        private string _editName = "";
        public string EditName
        {
            get => _editName;
            set
            { 
                _editName = value; 
                OnPropertyChanged(nameof(EditName));

                CanAction();
            }
        }

        private string _editCategory = "Electronics";
        public string EditCategory
        {
            get => _editCategory;
            set 
            { 
                _editCategory = value; 
                OnPropertyChanged(nameof(EditCategory));
                CanAction();
            }
        }

        private string _editQuantity = "0";
        public string EditQuantity
        {
            get => _editQuantity;
            set { 
                
                _editQuantity = int.TryParse(value, out int val) ? val.ToString() : "0"; 
                OnPropertyChanged(nameof(EditQuantity));
                CanAction();
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private void CanAction()
        {
            CanAdd();
            CanEditAsync();
            CanDeleteAsync();
        }
        private async Task ExecuteAddAsync()
        {
            var item = CreateItemFromInputs();
            await _itemActionCallback(Operations.Add, item);
            ResetInputs();
        }

        private async Task CanAdd()
        {
            bool canAdd = true;
            if(string.IsNullOrEmpty(EditName)) canAdd = false;
            if(string.IsNullOrEmpty(EditCategory)) canAdd = false;
            if (!int.TryParse(EditQuantity, out var quantity) || int.IsNegative(quantity)) 
                canAdd = false;
            if (canAdd != _canAdd)
            {
                _canAdd = canAdd;
                ((RelayCommandAsync)AddCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task CanEditAsync()
        {
            bool canEdit = true;
            if (ID == null || await _inventoryService.GetItemAsync(ID.Value) == null)
                canEdit = false;
            if (string.IsNullOrEmpty(EditName)) canEdit = false;
            if (string.IsNullOrEmpty(EditCategory)) canEdit = false;
            if (!int.TryParse(EditQuantity, out var quantity) || int.IsNegative(quantity))
                canEdit = false;

            if(canEdit != _canEdit)
            {
                _canEdit = canEdit;
                ((RelayCommandAsync)EditCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task CanDeleteAsync()
        {
            bool canDelete = true;
            if (ID == null || await _inventoryService.GetItemAsync(ID.Value) == null)
                canDelete = false;


            if (canDelete != _canDelete)
            {
                _canDelete = canDelete;
                ((RelayCommandAsync)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task ExecuteEditAsync()
        {
            var item = CreateItemFromInputs(CurrentItem.Id);
            await _itemActionCallback(Operations.Edit, item);
            ResetInputs();
        }

        private async Task ExecuteDeleteAsync()
        {
            await  _itemActionCallback(Operations.Delete, CurrentItem);
            ResetInputs();
        }

        private InventoryItem CreateItemFromInputs(Guid? id = null)
        {
            return new InventoryItem
            {
                Id = id ?? Guid.NewGuid(),
                Name = EditName,
                Category = EditCategory,
                Quantity = int.TryParse(EditQuantity, out int q) ? q : 0,
                Price = new Random().Next(10, 100)
            };
        }

        private void ResetInputs()
        {
            ID = null;
            EditName = string.Empty;
            EditCategory = "Electronics";
            EditQuantity = string.Empty;
        }

        public ObservableCollection<string> Categories { get; } =
            new ObservableCollection<string> { "Electronics", "Food", "Clothing" };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
