using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using InventorySrv.Dtos;
using InventorySrv.Services;

namespace InventorySrv.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCreateDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AuthenticateAsync(loginDto);

            if (token == null)
                return Unauthorized();

            return Ok(new { token });
        }
    }
}