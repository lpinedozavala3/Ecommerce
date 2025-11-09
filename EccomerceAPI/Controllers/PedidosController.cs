using System;
using System.Collections.Generic;
using System.Linq;
using Database.DTOs;
using EccomerceAPI.Contracts.Pedidos;
using EccomerceAPI.Helpers;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<Response<IReadOnlyList<PedidoResumenDto>>>> ObtenerPedidos([FromQuery] Guid clienteId)
        {
            if (clienteId == Guid.Empty)
            {
                var messageTitle = "La solicitud es inválida.";
                var errorMessage = "El identificador del cliente es obligatorio.";

                return BadRequest(new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            try
            {
                var pedidos = await _pedidosService.ObtenerPedidosAsync(tiendaId, clienteId);
                var messageTitle = "Pedidos recuperados correctamente.";

                return Ok(new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitle,
                    Data = pedidos
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener los pedidos.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpGet("direccion")]
        public async Task<ActionResult<Response<DireccionClienteDto?>>> ObtenerDireccion([FromQuery] Guid clienteId)
        {
            if (clienteId == Guid.Empty)
            {
                var messageTitle = "La solicitud es inválida.";
                var errorMessage = "El identificador del cliente es obligatorio.";

                return BadRequest(new Response<DireccionClienteDto?>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            try
            {
                var direccion = await _pedidosService.ObtenerDireccionAsync(tiendaId, clienteId);

                if (direccion is null)
                {
                    var messageTitle = "No se encontró una dirección registrada.";
                    var errorMessage = "El cliente no tiene dirección principal.";

                    return NotFound(new Response<DireccionClienteDto?>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = messageTitle,
                        Errors = new[] { errorMessage }
                    });
                }

                var messageTitleSuccess = "Dirección obtenida correctamente.";

                return Ok(new Response<DireccionClienteDto?>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitleSuccess,
                    Data = direccion
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener la dirección.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<DireccionClienteDto?>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpPut("direccion")]
        public async Task<ActionResult<Response<DireccionClienteDto>>> GuardarDireccion([FromQuery] Guid clienteId, [FromBody] UpsertDireccionRequest request)
        {
            if (clienteId == Guid.Empty)
            {
                var messageTitle = "La solicitud es inválida.";
                var errorMessage = "El identificador del cliente es obligatorio.";

                return BadRequest(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }

            if (!ModelState.IsValid)
            {
                var messageTitle = "Los datos de la dirección no son válidos.";
                var errores = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();

                return BadRequest(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = messageTitle,
                    Errors = errores
                });
            }

            var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
            try
            {
                var direccion = await _pedidosService.GuardarDireccionAsync(tiendaId, clienteId, request);
                var messageTitle = "Dirección guardada correctamente.";

                return Ok(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitle,
                    Data = direccion
                });
            }
            catch (InvalidOperationException ex)
            {
                var messageTitle = "No se pudo guardar la dirección.";
                var errorMessage = ex.Message;

                return NotFound(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status404NotFound,
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
                var messageTitle = "Error al guardar la dirección.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
