using System;
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
            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _authService.RegistrarAsync(request, tiendaId);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<AuthResponse>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo registrar al cliente."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<AuthResponse>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Cliente registrado correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al registrar al cliente";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

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
            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _authService.IniciarSesionAsync(request, tiendaId);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<AuthResponse>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo iniciar sesión."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<AuthResponse>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Inicio de sesión exitoso."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al iniciar sesión";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

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
