using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class InventoryUpdateDto
    {
        [Required]
        public bool Rain { get; set; }
    }
}
