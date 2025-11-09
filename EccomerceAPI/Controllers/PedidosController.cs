using System;
using System.Collections.Generic;
using Database.DTOs;
using EccomerceAPI.Contracts.Pedidos;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public sealed class PedidosController : ControllerBase
    {
        private readonly IPedidosService _pedidosService;

        public PedidosController(IPedidosService pedidosService)
        {
            _pedidosService = pedidosService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IReadOnlyList<PedidoResumenDto>>>> ObtenerPedidos([FromQuery] Guid clienteId)
        {
            var response = await _pedidosService.ObtenerPedidosAsync(HttpContext, clienteId);
            return StatusCode(response.Status, response);
        }

        [HttpGet("direccion")]
        public async Task<ActionResult<Response<DireccionClienteDto>>> ObtenerDireccion([FromQuery] Guid clienteId)
        {
            var response = await _pedidosService.ObtenerDireccionAsync(HttpContext, clienteId);
            return StatusCode(response.Status, response);
        }

        [HttpPut("direccion")]
        public async Task<ActionResult<Response<DireccionClienteDto>>> GuardarDireccion([
            FromQuery] Guid clienteId,
            [FromBody] UpsertDireccionRequest request)
        {
            var response = await _pedidosService.GuardarDireccionAsync(HttpContext, clienteId, request);
            return StatusCode(response.Status, response);
        }
    }
}
