using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.DTOs;
using Database.Filters;
using Microsoft.AspNetCore.Http;

namespace EccomerceAPI.Interfaces
{
    public interface ICatalogoService
    {
        Task<Response<PagedResponse<List<ProductoDto>>>> ObtenerProductosAsync(HttpContext httpContext, ProductoFilter filter, PaginationFilter pagination);
        Task<Response<ProductoDetalleDto>> ObtenerDetalleAsync(HttpContext httpContext, Guid productoId);
        Task<Response<IReadOnlyList<CategoriaDto>>> ObtenerCategoriasAsync(HttpContext httpContext);
        Task<Response<Dictionary<Guid, ProductoDto>>> ObtenerProductosPorIdsAsync(Guid emisorId, IEnumerable<Guid> ids);
    }
}
