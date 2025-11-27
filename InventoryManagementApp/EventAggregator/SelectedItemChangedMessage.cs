using Shared.Models;

namespace InventoryManagementApp.EventAggregator
{
    public class SelectedItemChangedMessage
    {
        public InventoryItem? SelectedItem { get; }
        public SelectedItemChangedMessage(InventoryItem? item)
        {
            SelectedItem = item;
        }
    }
}
