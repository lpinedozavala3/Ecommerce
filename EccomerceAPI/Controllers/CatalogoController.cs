// EccomerceAPI/Controllers/CatalogoController.cs
using Database.DTOs;
using Database.Filters;
using Database.Models;
using EccomerceAPI.Helpers;
using EccomerceAPI.Interfaces;
using EccomerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/catalogo")]
    public sealed class CatalogoController : ControllerBase
    {
        private readonly contextApp _db;
        private readonly ITenantResolver _tenant;
        private readonly ICatalogoService _catalogo;
        private readonly IUriService _uriService;

        public CatalogoController(contextApp db, ITenantResolver tenant, ICatalogoService catalogo, IUriService uriService)
        {
            _db = db;
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

            var (items, total) = await _catalogo.ListFilter(productoFilter, valid);
            var response = PaginationHelper.CreatePagedReponse(items, valid, total, _uriService, route);
            return Ok(response);
        }

        [HttpGet("productos/{productoId:guid}")]
        public async Task<ActionResult<ProductoDetalleDto>> ObtenerDetalle(Guid productoId)
        {
            var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);
            var detalle = await _catalogo.ObtenerDetalle(productoId, emisorId);
            if (detalle is null)
            {
                return NotFound();
            }

            return Ok(detalle);
        }

        // GET /api/catalogo/categorias
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetCategorias()
        {
            var (_, emisorId) = await _tenant.ResolveAsync(HttpContext);

            var categorias = await _db.Categoria
                .AsNoTracking()
                .Where(c => c.IdEmisor == emisorId)
                .Select(c => new CategoriaDto
                {
                    IdCategoria     = c.IdCategoria,
                    NombreCategoria = c.NombreCategoria,
                    SlugCategoria   = c.SlugCategoria,
                    ProductosVisibles = c.IdProductos.Count(p => p.VisibleEnTienda && p.EmisorId == emisorId)
                })
                .OrderBy(c => c.NombreCategoria)
                .ToListAsync();

            return Ok(categorias);
        }
    }
}
