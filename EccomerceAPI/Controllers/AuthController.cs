using Database.DTOs;
using EccomerceAPI.Contracts.Auth;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Response<AuthResponse>>> Registrar([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegistrarAsync(HttpContext, request);
            return StatusCode(response.Status, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<AuthResponse>>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.IniciarSesionAsync(HttpContext, request);
            return StatusCode(response.Status, response);
        }
    }
}
