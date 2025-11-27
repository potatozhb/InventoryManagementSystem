using InventorySrv.Dtos;

namespace InventorySrv.Services
{
    public interface IAuthService
    {
        Task<string?> AuthenticateAsync(UserCreateDto loginDto);
    }
}