
using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class InventoryReadDto
    {
        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public bool Rain { get; set; }
    }
}