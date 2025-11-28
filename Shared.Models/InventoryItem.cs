using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class InventoryItem
    {
        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;

        public decimal Price { get; set; }
    }
}
