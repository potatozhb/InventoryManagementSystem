
using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Models
{
    public class InventoryItem1
    {
        private static readonly DateTime utcNow = DateTime.UtcNow;

        [Key]
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();//uuid

        [Required]
        [StringLength(100)]
        public string UserId { get; set; }

        [Required]
        public bool Rain { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = utcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = utcNow;
    }
}