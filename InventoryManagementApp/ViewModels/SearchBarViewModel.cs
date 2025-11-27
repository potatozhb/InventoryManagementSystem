
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace InventoryManagementApp.ViewModels
{
    public class SearchBarViewModel : INotifyPropertyChanged
    {
        private string _searchName = "";
        public string SearchName
        {
            get => _searchName;
            set 
            {
                if (_searchName != value)
                {
                    _searchName = value;
                    OnPropertyChanged(nameof(SearchName));
                }
            }
        }

        private string _searchCategory = "All";
        public string SearchCategory
        {
            get => _searchCategory;
            set 
            {
                if(_searchCategory != value)
                {
                    _searchCategory = value; 
                    OnPropertyChanged(nameof(SearchCategory));
                }
            }
        }

        public ObservableCollection<string> Categories { get; } = 
            new ObservableCollection<string> { "All", "Electronics", "Food", "Clothing" };

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
