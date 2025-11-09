using System;
using Database.DTOs;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    public sealed class CarritoController : ControllerBase
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ICarritoService _carritoService;

        public CarritoController(ITenantResolver tenantResolver, ICarritoService carritoService)
        {
            _tenantResolver = tenantResolver;
            _carritoService = carritoService;
        }

        [HttpPost("resumen")]
        public async Task<ActionResult<Response<CartSummaryResponse>>> ObtenerResumen([FromBody] CartSummaryRequest request)
        {
            try
            {
                var (_, emisorId) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _carritoService.ObtenerResumenAsync(emisorId, request);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<CartSummaryResponse>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo calcular el resumen del carrito."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<CartSummaryResponse>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Resumen del carrito calculado correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener el resumen del carrito";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<CartSummaryResponse>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
