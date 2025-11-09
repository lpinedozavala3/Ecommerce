using Database.DTOs;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    public sealed class CarritoController : ControllerBase
    {
        private readonly ICarritoService _carritoService;

        public CarritoController(ICarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpPost("resumen")]
        public async Task<ActionResult<Response<CartSummaryResponse>>> ObtenerResumen([FromBody] CartSummaryRequest request)
        {
            var response = await _carritoService.ObtenerResumenAsync(HttpContext, request);
            return StatusCode(response.Status, response);
        }
    }
}
