using Database.DTOs;
using EccomerceAPI.Contracts.Auth;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<Response<AuthResponse>>> Registrar([FromBody] RegisterRequest request)
        {
            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);

            try
            {
                var resp = await _authService.RegistrarAsync(request, tiendaId);
                var messageTitle = "Registro exitoso.";

                return Ok(new Response<AuthResponse>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitle,
                    Data = resp
                });
            }
            catch (InvalidOperationException ex)
            {
                var messageTitle = "No se pudo completar el registro.";
                var errorMessage = ex.Message;

                return Conflict(new Response<AuthResponse>
                {
                    Status = StatusCodes.Status409Conflict,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al registrar la cuenta.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<AuthResponse>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<Response<AuthResponse>>> Login([FromBody] LoginRequest request)
        {
            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);

            try
            {
                var resp = await _authService.IniciarSesionAsync(request, tiendaId);
                var messageTitle = "Autenticación exitosa.";

                return Ok(new Response<AuthResponse>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitle,
                    Data = resp
                });
            }
            catch (InvalidOperationException ex)
            {
                var messageTitle = "Las credenciales no son válidas.";
                var errorMessage = ex.Message;

                return Unauthorized(new Response<AuthResponse>
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al iniciar sesión.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<AuthResponse>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
