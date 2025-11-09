// EccomerceAPI/Controllers/CatalogoController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Database.DTOs;
using Database.Filters;
using EccomerceAPI.Helpers;
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
        private readonly IUriService _uriService;

        public CatalogoController(ITenantResolver tenant, ICatalogoService catalogo, IUriService uriService)
        {
            _tenant = tenant;
            _catalogo = catalogo;
            _uriService = uriService;
        }

        // GET /api/catalogo/productos/list?searchText=...&categoriaId=...&pageNumber=1&pageSize=12
        [HttpGet("productos/list")]
        public async Task<ActionResult<PagedResponse<List<ProductoDto>>>> ProductosList(
            [FromQuery] ProductoFilter productoFilter,
            [FromQuery] PaginationFilter pagination)
        {
            var route = Request.Path.Value;
            var valid = new PaginationFilter(pagination.PageNumber, pagination.PageSize);

            var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
            productoFilter.EmisorId = emisorId;
            productoFilter.VisibleEnTienda = true;

            try
            {
                var (items, total) = await _catalogo.ListFilter(productoFilter, valid);
                var response = PaginationHelper.CreatePagedReponse(items, valid, total, _uriService, route);
                response.Message = "Productos obtenidos correctamente.";
                response.Status = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener los productos.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<List<ProductoDto>>
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
            var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
            try
            {
                var detalle = await _catalogo.ObtenerDetalle(productoId, emisorId);
                if (detalle is null)
                {
                    var messageTitle = "Producto no encontrado.";
                    var errorMessage = "No existe un producto con el identificador indicado.";

                    return NotFound(new Response<ProductoDetalleDto>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = messageTitle,
                        Errors = new[] { errorMessage }
                    });
                }

                var successMessage = "Producto obtenido correctamente.";

                return Ok(new Response<ProductoDetalleDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = successMessage,
                    Data = detalle
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener el detalle del producto.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<ProductoDetalleDto>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }

        // GET /api/catalogo/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<Response<IReadOnlyList<CategoriaDto>>>> GetCategorias()
        {
            var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
            try
            {
                var categorias = await _catalogo.ObtenerCategorias(emisorId);
                var messageTitle = "Categorías obtenidas correctamente.";

                return Ok(new Response<IReadOnlyList<CategoriaDto>>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageTitle,
                    Data = categorias
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener las categorías.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<IReadOnlyList<CategoriaDto>>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
