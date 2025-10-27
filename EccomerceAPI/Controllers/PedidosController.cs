using System;
using System.Collections.Generic;
using EccomerceAPI.Contracts.Pedidos;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public sealed class PedidosController : ControllerBase
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly IPedidosService _pedidosService;

        public PedidosController(ITenantResolver tenantResolver, IPedidosService pedidosService)
        {
            _tenantResolver = tenantResolver;
            _pedidosService = pedidosService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<PedidoResumenDto>>> ObtenerPedidos([FromQuery] Guid clienteId)
        {
            if (clienteId == Guid.Empty)
            {
                return BadRequest(new { message = "El identificador del cliente es obligatorio." });
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            var pedidos = await _pedidosService.ObtenerPedidosAsync(tiendaId, clienteId);
            return Ok(pedidos);
        }

        [HttpGet("direccion")]
        public async Task<ActionResult<DireccionClienteDto>> ObtenerDireccion([FromQuery] Guid clienteId)
        {
            if (clienteId == Guid.Empty)
            {
                return BadRequest(new { message = "El identificador del cliente es obligatorio." });
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            var direccion = await _pedidosService.ObtenerDireccionAsync(tiendaId, clienteId);

            if (direccion is null)
            {
                return NoContent();
            }

            return Ok(direccion);
        }

        [HttpPut("direccion")]
        public async Task<ActionResult<DireccionClienteDto>> GuardarDireccion([FromQuery] Guid clienteId, [FromBody] UpsertDireccionRequest request)
        {
            if (clienteId == Guid.Empty)
            {
                return BadRequest(new { message = "El identificador del cliente es obligatorio." });
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            try
            {
                var direccion = await _pedidosService.GuardarDireccionAsync(tiendaId, clienteId, request);
                return Ok(direccion);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
