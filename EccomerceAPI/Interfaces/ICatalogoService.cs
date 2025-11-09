using Database.DTOs;
using Database.Filters;
using EccomerceAPI.Common.Results;

namespace EccomerceAPI.Interfaces
{
    public interface ICatalogoService
    {
        Task<ServiceResult<PagedResponse<List<ProductoDto>>>> ObtenerProductosAsync(
            Guid emisorId,
            ProductoFilter filter,
            PaginationFilter pagination,
            string route);

        Task<ServiceResult<ProductoDetalleDto>> ObtenerDetalleAsync(Guid productoId, Guid emisorId);

        Task<ServiceResult<IReadOnlyDictionary<Guid, ProductoDto>>> ObtenerPorIdsAsync(IEnumerable<Guid> ids, Guid emisorId);

        Task<ServiceResult<List<CategoriaDto>>> ObtenerCategoriasAsync(Guid emisorId);
    }
}
