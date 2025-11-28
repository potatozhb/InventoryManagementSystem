using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class InventoryUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
