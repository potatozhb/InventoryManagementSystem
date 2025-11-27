
using System.ComponentModel.DataAnnotations;

namespace InventorySrv.Dtos
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        public string? Password { get; set; }

    }
}