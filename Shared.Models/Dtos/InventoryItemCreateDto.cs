using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Dtos
{
    public class InventoryItemCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Quantity { get; set; }

    }
}