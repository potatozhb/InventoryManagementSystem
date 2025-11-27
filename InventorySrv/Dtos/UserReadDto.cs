
using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class UserReadDto
    {
        [Required]
        [StringLength(100)]
        public string? Username { get; set; }
    }
}