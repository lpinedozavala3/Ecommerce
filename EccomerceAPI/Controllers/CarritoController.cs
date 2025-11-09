using System.Linq;
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
            if (!ModelState.IsValid)
            {
                var messageTitle = "La solicitud de resumen es inválida.";
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();

                return BadRequest(new Response<CartSummaryResponse>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = messageTitle,
                    Errors = errores
                });
            }

            var (_, emisorId) = await _tenantResolver.ResolveAsync(HttpContext);

            try
            {
                var resumen = await _carritoService.ObtenerResumenAsync(emisorId, request);

                if (resumen.Items.Count == 0)
                {
                    var messageTitle = "Ninguno de los productos está disponible.";
                    var errorMessage = "Actualiza tu carrito e inténtalo nuevamente.";

                    return BadRequest(new Response<CartSummaryResponse>
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Message = messageTitle,
                        Errors = new[] { errorMessage }
                    });
                }

                var successMessage = "Resumen de carrito generado correctamente.";

                return Ok(new Response<CartSummaryResponse>
                {
                    Status = StatusCodes.Status200OK,
                    Message = successMessage,
                    Data = resumen
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al generar el resumen del carrito.";

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
