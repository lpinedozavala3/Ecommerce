using Database.DTOs;
using Database.Filters;

namespace EccomerceAPI.Interfaces
{
    public interface ICatalogoService
    {
        Task<(bool response, int status, string message, PagedResponse<List<ProductoDto>>? data)> ObtenerProductosAsync(
            Guid emisorId,
            ProductoFilter filter,
            PaginationFilter pagination,
            string route);

        Task<(bool response, int status, string message, ProductoDetalleDto? data)> ObtenerDetalleAsync(Guid productoId, Guid emisorId);

        Task<(bool response, int status, string message, IReadOnlyDictionary<Guid, ProductoDto>? data)> ObtenerPorIdsAsync(IEnumerable<Guid> ids, Guid emisorId);

        Task<(bool response, int status, string message, List<CategoriaDto>? data)> ObtenerCategoriasAsync(Guid emisorId);
    }
}
