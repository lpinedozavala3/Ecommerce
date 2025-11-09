using System;
using System.Collections.Generic;
using System.Linq;
using Database.DTOs;
using Database.Filters;
using Database.Models;
using EccomerceAPI.Helpers;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EccomerceAPI.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly contextApp _db;
        private readonly ITenantResolver _tenantResolver;
        private readonly IUriService _uriService;

        public CatalogoService(contextApp db, ITenantResolver tenantResolver, IUriService uriService)
        {
            _db = db;
            _tenantResolver = tenantResolver;
            _uriService = uriService;
        }

        public async Task<Response<PagedResponse<List<ProductoDto>>>> ObtenerProductosAsync(HttpContext httpContext, ProductoFilter filter, PaginationFilter pagination)
        {
            var response = new Response<PagedResponse<List<ProductoDto>>>();

            try
            {
                var (_, emisorId) = await _tenantResolver.ResolveAsync(httpContext);
                var validPagination = new PaginationFilter(pagination.PageNumber, pagination.PageSize);
                filter.EmisorId = emisorId;
                filter.VisibleEnTienda = true;

                var (items, total) = await ListFilterAsync(filter, validPagination);
                var route = httpContext.Request.Path.Value ?? string.Empty;
                var paged = PaginationHelper.CreatePagedReponse(items, validPagination, total, _uriService, route);

                response.Status = StatusCodes.Status200OK;
                response.Message = total > 0 ? "Productos recuperados correctamente." : "No se encontraron productos.";
                response.Data = paged;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener el catálogo." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener el catálogo.";
                return response;
            }
        }

        public async Task<Response<ProductoDetalleDto>> ObtenerDetalleAsync(HttpContext httpContext, Guid productoId)
        {
            var response = new Response<ProductoDetalleDto>();

            try
            {
                var (_, emisorId) = await _tenantResolver.ResolveAsync(httpContext);
                var detalle = await _db.Productos
                    .AsNoTracking()
                    .Where(p => p.ProductoId == productoId && p.EmisorId == emisorId && p.VisibleEnTienda)
                    .Select(p => new ProductoDetalleDto
                    {
                        ProductoId = p.ProductoId,
                        NombrePublico = p.NombrePublico,
                        Precio = (decimal)p.Precio,
                        Stock = p.Stock,
                        ImagenBase64 = p.ImagenBase64,
                        VisibleEnTienda = p.VisibleEnTienda,
                        Destacado = null,
                        Novedad = p.Novedad,
                        Exento = p.Exento,
                        DescripcionCorta = p.DescripcionCorta,
                        DescripcionLarga = p.DescripcionLarga,
                        CodigoBarra = p.CodigoBarra,
                        UnidadMedida = p.UnidadMedida,
                        Activo = p.Activo,
                        Categorias = p.IdCategoria
                            .OrderBy(c => c.NombreCategoria)
                            .Select(c => new CategoriaDto
                            {
                                IdCategoria = c.IdCategoria,
                                NombreCategoria = c.NombreCategoria,
                                SlugCategoria = c.SlugCategoria
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync();

                if (detalle is null)
                {
                    response.Status = StatusCodes.Status404NotFound;
                    response.Message = "El producto no se encuentra disponible.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                response.Status = StatusCodes.Status200OK;
                response.Message = "Detalle obtenido correctamente.";
                response.Data = detalle;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener el detalle del producto." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener el detalle del producto.";
                return response;
            }
        }

        public async Task<Response<IReadOnlyList<CategoriaDto>>> ObtenerCategoriasAsync(HttpContext httpContext)
        {
            var response = new Response<IReadOnlyList<CategoriaDto>>();

            try
            {
                var (_, emisorId) = await _tenantResolver.ResolveAsync(httpContext);

                var categorias = await _db.Categoria
                    .AsNoTracking()
                    .Where(c => c.IdEmisor == emisorId)
                    .Select(c => new CategoriaDto
                    {
                        IdCategoria = c.IdCategoria,
                        NombreCategoria = c.NombreCategoria,
                        SlugCategoria = c.SlugCategoria,
                        ProductosVisibles = c.IdProductos.Count(p => p.VisibleEnTienda && p.EmisorId == emisorId)
                    })
                    .OrderBy(c => c.NombreCategoria)
                    .ToListAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = categorias.Count > 0 ? "Categorías obtenidas correctamente." : "No existen categorías disponibles.";
                response.Data = categorias;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener las categorías." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener las categorías.";
                return response;
            }
        }

        public async Task<Response<Dictionary<Guid, ProductoDto>>> ObtenerProductosPorIdsAsync(Guid emisorId, IEnumerable<Guid> ids)
        {
            var response = new Response<Dictionary<Guid, ProductoDto>>();

            try
            {
                var hash = ids.ToHashSet();
                var productos = await _db.Productos
                    .AsNoTracking()
                    .Where(p => hash.Contains(p.ProductoId) && p.EmisorId == emisorId && p.VisibleEnTienda)
                    .ToDictionaryAsync(
                        p => p.ProductoId,
                        p => new ProductoDto
                        {
                            ProductoId = p.ProductoId,
                            NombrePublico = p.NombrePublico,
                            Precio = (decimal)p.Precio,
                            Stock = p.Stock,
                            ImagenBase64 = p.ImagenBase64,
                            VisibleEnTienda = p.VisibleEnTienda,
                            Destacado = null,
                            Novedad = p.Novedad,
                            Exento = p.Exento,
                            Categorias = p.IdCategoria
                                .OrderBy(c => c.NombreCategoria)
                                .Select(c => new CategoriaDto
                                {
                                    IdCategoria = c.IdCategoria,
                                    NombreCategoria = c.NombreCategoria,
                                    SlugCategoria = c.SlugCategoria
                                })
                                .ToList()
                        });

                response.Status = StatusCodes.Status200OK;
                response.Message = "Productos obtenidos correctamente.";
                response.Data = productos;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener los productos." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener los productos.";
                return response;
            }
        }

        private async Task<(List<ProductoDto> Items, int Total)> ListFilterAsync(ProductoFilter filter, PaginationFilter pagination)
        {
            var expr = filter.BuildFilter();
            var total = await _db.Productos.AsNoTracking().Where(expr).CountAsync();

            var query = _db.Productos
                .AsNoTracking()
                .Where(expr)
                .OrderBy(p => p.NombrePublico ?? p.Nombre)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(p => new ProductoDto
                {
                    ProductoId = p.ProductoId,
                    NombrePublico = p.NombrePublico,
                    Precio = (decimal)p.Precio,
                    Stock = p.Stock,
                    ImagenBase64 = p.ImagenBase64,
                    VisibleEnTienda = p.VisibleEnTienda,
                    Novedad = p.Novedad,
                    Exento = p.Exento,
                    Categorias = p.IdCategoria
                        .OrderBy(c => c.NombreCategoria)
                        .Select(c => new CategoriaDto
                        {
                            IdCategoria = c.IdCategoria,
                            NombreCategoria = c.NombreCategoria,
                            SlugCategoria = c.SlugCategoria
                        })
                        .ToList()
                });

            var items = await query.ToListAsync();
            return (items, total);
        }
    }
}
