using Database.DTOs;
using Database.Filters;
using Database.Models;
using EccomerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EccomerceAPI.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly contextApp _db;
        public CatalogoService(contextApp db) => _db = db;

        public async Task<(List<ProductoDto> Items, int Total)> ListFilter(ProductoFilter filter, PaginationFilter pagination)
        {
            var expr = filter.BuildFilter();

            // total
            var total = await _db.Productos.AsNoTracking().Where(expr).CountAsync();

            // data
            var query = _db.Productos
                .AsNoTracking()
                .Where(expr)
                .OrderBy(p => p.NombrePublico ?? p.Nombre)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Select(p => new ProductoDto
                {
                    ProductoId      = p.ProductoId,
                    NombrePublico   = p.NombrePublico,
                    Precio          = (decimal)p.Precio,
                    Stock           = p.Stock,
                    ImagenBase64    = p.ImagenBase64,
                    VisibleEnTienda = p.VisibleEnTienda,
                    Exento          = p.Exento,
                    Categorias = p.IdCategoria
                        .OrderBy(c => c.NombreCategoria)
                        .Select(c => new CategoriaDto
                        {
                            IdCategoria     = c.IdCategoria,
                            NombreCategoria = c.NombreCategoria,
                            SlugCategoria   = c.SlugCategoria
                        })
                        .ToList()
                });

            var items = await query.ToListAsync();
            return (items, total);
        }

        public async Task<ProductoDetalleDto?> ObtenerDetalle(Guid productoId, Guid emisorId)
        {
            return await _db.Productos
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
        }

        public async Task<Dictionary<Guid, ProductoDto>> ObtenerPorIds(IEnumerable<Guid> ids, Guid emisorId)
        {
            var hash = ids.ToHashSet();
            return await _db.Productos
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
        }
    }
}
