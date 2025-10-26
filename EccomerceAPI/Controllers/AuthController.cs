using EccomerceAPI.Contracts.Auth;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly IAuthService _authService;

        public AuthController(ITenantResolver tenantResolver, IAuthService authService)
        {
            _tenantResolver = tenantResolver;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Registrar([FromBody] RegisterRequest request)
        {
            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);

            try
            {
                var resp = await _authService.RegistrarAsync(request, tiendaId);
                return Ok(resp);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);

            try
            {
                var resp = await _authService.IniciarSesionAsync(request, tiendaId);
                return Ok(resp);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
