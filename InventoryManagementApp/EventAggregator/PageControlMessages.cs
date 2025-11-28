using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApp.EventAggregator
{
    public class PageChangedMessage
    {
        public int NewPage { get; set; }
    }

    public class RequestPageInfoMessage { }

    public class ProvidePageInfoMessage
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
