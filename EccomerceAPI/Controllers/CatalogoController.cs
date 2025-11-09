using System.Collections.Generic;
using Database.DTOs;
using Database.Filters;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public sealed class CatalogoController : ControllerBase
    {
        private readonly ICatalogoService _catalogoService;

        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet("productos/list")]
        public async Task<ActionResult<Response<PagedResponse<List<ProductoDto>>>>> ProductosList([
            FromQuery] ProductoFilter productoFilter,
            [FromQuery] PaginationFilter pagination)
        {
            var response = await _catalogoService.ObtenerProductosAsync(HttpContext, productoFilter, pagination);
            return StatusCode(response.Status, response);
        }

        [HttpGet("productos/{productoId:guid}")]
        public async Task<ActionResult<Response<ProductoDetalleDto>>> ObtenerDetalle(Guid productoId)
        {
            var response = await _catalogoService.ObtenerDetalleAsync(HttpContext, productoId);
            return StatusCode(response.Status, response);
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<Response<IReadOnlyList<CategoriaDto>>>> GetCategorias()
        {
            var response = await _catalogoService.ObtenerCategoriasAsync(HttpContext);
            return StatusCode(response.Status, response);
        }
    }
}
