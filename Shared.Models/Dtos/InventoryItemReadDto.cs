namespace Shared.Models.Dtos
{
    public class InventoryItemReadDto
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal Price { get; set; }
    }
}
