using Shared.Models;

namespace InventorySrv.Dtos
{
    public class InventoryReadResponse
    {
        public PagedResultDto<InventoryItem> Data { get; set; }
    }
}
