using Database.DTOs;
using Database.Filters;
using Database.Models;
using EccomerceAPI.Helpers;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly contextApp _db;
        private readonly IUriService _uriService;

        public CatalogoService(contextApp db, IUriService uriService)
        {
            _db = db;
            _uriService = uriService;
        }

        public async Task<(bool response, int status, string message, PagedResponse<List<ProductoDto>>? data)> ObtenerProductosAsync(
            Guid emisorId,
            ProductoFilter filter,
            PaginationFilter pagination,
            string route)
        {
            try
            {
                var validFilter = new PaginationFilter(pagination.PageNumber, pagination.PageSize);
                filter.EmisorId = emisorId;
                filter.VisibleEnTienda = true;

                var expr = filter.BuildFilter();

                var total = await _db.Productos
                    .AsNoTracking()
                    .Where(expr)
                    .CountAsync();

                var items = await _db.Productos
                    .AsNoTracking()
                    .Where(expr)
                    .OrderBy(p => p.NombrePublico ?? p.Nombre)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
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
                    })
                    .ToListAsync();

                var response = PaginationHelper.CreatePagedReponse(items, validFilter, total, _uriService, route);
                return (true, StatusCodes.Status200OK, "Productos obtenidos correctamente.", response);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo obtener el listado de productos.", null);
            }
        }

        public async Task<(bool response, int status, string message, ProductoDetalleDto? data)> ObtenerDetalleAsync(Guid productoId, Guid emisorId)
        {
            try
            {
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
                            .ToList(),
                    })
                    .FirstOrDefaultAsync();

                if (detalle is null)
                {
                    return (false, StatusCodes.Status404NotFound, "El producto solicitado no existe.", null);
                }

                return (true, StatusCodes.Status200OK, "Detalle del producto obtenido correctamente.", detalle);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo obtener el detalle del producto.", null);
            }
        }

        public async Task<(bool response, int status, string message, IReadOnlyDictionary<Guid, ProductoDto>? data)> ObtenerPorIdsAsync(IEnumerable<Guid> ids, Guid emisorId)
        {
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
                                .ToList(),
                        });

                return (true, StatusCodes.Status200OK, "Productos obtenidos correctamente.", productos);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudieron obtener los productos solicitados.", null);
            }
        }

        public async Task<(bool response, int status, string message, List<CategoriaDto>? data)> ObtenerCategoriasAsync(Guid emisorId)
        {
            try
            {
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

                return (true, StatusCodes.Status200OK, "Categorías obtenidas correctamente.", categorias);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudieron obtener las categorías.", null);
            }
        }
    }
}
