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

                if (!result.response)
                {
                    var status = result.status;
                    var message = string.IsNullOrWhiteSpace(result.message)
                        ? "No se pudo calcular el resumen del carrito."
                        : result.message;

                    return StatusCode(status, new Response<CartSummaryResponse>
                    {
                        Status = status,
                        Message = message,
                        Errors = new[] { message }
                    });
                }

                return StatusCode(result.status, new Response<CartSummaryResponse>
                {
                    Status = result.status,
                    Message = string.IsNullOrWhiteSpace(result.message)
                        ? "Resumen del carrito calculado correctamente."
                        : result.message,
                    Data = result.data,
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
