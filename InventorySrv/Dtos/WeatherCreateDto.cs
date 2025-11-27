
using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class InventoryCreateDto
    {
        [Required]
        public bool? Rain { get; set; } //need nullable value to check the field exist
    }
}