namespace InventorySrv.Dtos
{
    public class InventoryReadResponse
    {
        public IEnumerable<InventoryReadDto> Data { get; set; }
    }
}
