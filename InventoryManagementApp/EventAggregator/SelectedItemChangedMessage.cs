using InventoryManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
