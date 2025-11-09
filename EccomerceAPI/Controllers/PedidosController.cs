using System;
using System.Collections.Generic;
using System.Linq;
using Database.DTOs;
using EccomerceAPI.Contracts.Pedidos;
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
                return BadRequest(new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "El identificador del cliente es obligatorio.",
                    Errors = new[] { "Debe proporcionar un identificador de cliente válido." }
                });
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _pedidosService.ObtenerPedidosAsync(tiendaId, clienteId);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<IReadOnlyList<PedidoResumenDto>>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudieron obtener los pedidos."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Pedidos obtenidos correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener los pedidos";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IReadOnlyList<PedidoResumenDto>>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpGet("direccion")]
        public async Task<ActionResult<Response<DireccionClienteDto>>> ObtenerDireccion([FromQuery] Guid clienteId)
        {
            if (clienteId == Guid.Empty)
            {
                return BadRequest(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "El identificador del cliente es obligatorio.",
                    Errors = new[] { "Debe proporcionar un identificador de cliente válido." }
                });
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _pedidosService.ObtenerDireccionAsync(tiendaId, clienteId);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<DireccionClienteDto>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo obtener la dirección."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<DireccionClienteDto>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Dirección obtenida correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener la dirección";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<DireccionClienteDto>
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
                return BadRequest(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "El identificador del cliente es obligatorio.",
                    Errors = new[] { "Debe proporcionar un identificador de cliente válido." }
                });
            }

            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? "Dato inválido." : e.ErrorMessage)
                    .ToArray();

                return BadRequest(new Response<DireccionClienteDto>
                {
                    Status = StatusCodes.Status400BadRequest,
                    Message = "La solicitud contiene datos inválidos.",
                    Errors = validationErrors.Length > 0 ? validationErrors : new[] { "Revise los datos enviados." }
                });
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(HttpContext);
                var result = await _pedidosService.GuardarDireccionAsync(tiendaId, clienteId, request);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<DireccionClienteDto>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo guardar la dirección."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<DireccionClienteDto>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Dirección guardada correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al guardar la dirección";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

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
