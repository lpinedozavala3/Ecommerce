using System;
using System.Collections.Generic;
using Database.DTOs;
using Database.Filters;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public sealed class CatalogoController : ControllerBase
    {
        private readonly ITenantResolver _tenant;
        private readonly ICatalogoService _catalogo;

        public CatalogoController(ITenantResolver tenant, ICatalogoService catalogo)
        {
            _tenant = tenant;
            _catalogo = catalogo;
        }

        [HttpGet("productos/list")]
        public async Task<ActionResult<Response<PagedResponse<List<ProductoDto>>>>> ProductosList(
            [FromQuery] ProductoFilter productoFilter,
            [FromQuery] PaginationFilter pagination)
        {
            var route = Request.Path.Value ?? string.Empty;

            try
            {
                var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
                var result = await _catalogo.ObtenerProductosAsync(emisorId, productoFilter, pagination, route);

                if (!result.response)
                {
                    var status = result.status;
                    var message = string.IsNullOrWhiteSpace(result.message)
                        ? "No se pudo obtener el catálogo."
                        : result.message;

                    return StatusCode(status, new Response<PagedResponse<List<ProductoDto>>>
                    {
                        Status = status,
                        Message = message,
                        Errors = new[] { message }
                    });
                }

                return StatusCode(result.status, new Response<PagedResponse<List<ProductoDto>>>
                {
                    Status = result.status,
                    Message = string.IsNullOrWhiteSpace(result.message)
                        ? "Catálogo obtenido correctamente."
                        : result.message,
                    Data = result.data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener el catálogo";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<PagedResponse<List<ProductoDto>>>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpGet("productos/{productoId:guid}")]
        public async Task<ActionResult<Response<ProductoDetalleDto>>> ObtenerDetalle(Guid productoId)
        {
            try
            {
                var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
                var result = await _catalogo.ObtenerDetalleAsync(productoId, emisorId);

                if (!result.response)
                {
                    var status = result.status;
                    var message = string.IsNullOrWhiteSpace(result.message)
                        ? "No se pudo obtener el detalle del producto."
                        : result.message;

                    return StatusCode(status, new Response<ProductoDetalleDto>
                    {
                        Status = status,
                        Message = message,
                        Errors = new[] { message }
                    });
                }

                return StatusCode(result.status, new Response<ProductoDetalleDto>
                {
                    Status = result.status,
                    Message = string.IsNullOrWhiteSpace(result.message)
                        ? "Detalle del producto obtenido correctamente."
                        : result.message,
                    Data = result.data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener el detalle del producto";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<ProductoDetalleDto>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<Response<List<CategoriaDto>>>> GetCategorias()
        {
            try
            {
                var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
                var result = await _catalogo.ObtenerCategoriasAsync(emisorId);

                if (!result.response)
                {
                    var status = result.status;
                    var message = string.IsNullOrWhiteSpace(result.message)
                        ? "No se pudieron obtener las categorías."
                        : result.message;

                    return StatusCode(status, new Response<List<CategoriaDto>>
                    {
                        Status = status,
                        Message = message,
                        Errors = new[] { message }
                    });
                }

                return StatusCode(result.status, new Response<List<CategoriaDto>>
                {
                    Status = result.status,
                    Message = string.IsNullOrWhiteSpace(result.message)
                        ? "Categorías obtenidas correctamente."
                        : result.message,
                    Data = result.data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener las categorías";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<List<CategoriaDto>>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
