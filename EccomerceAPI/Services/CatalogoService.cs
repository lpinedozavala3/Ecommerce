using Database.DTOs;
using Database.Filters;
using Database.Models;
using EccomerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    }
}
