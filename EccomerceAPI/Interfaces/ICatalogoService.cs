using Database.DTOs;
using Database.Filters;

namespace EccomerceAPI.Interfaces
{
    public interface ICatalogoService
    {
        Task<(List<ProductoDto> Items, int Total)> ListFilter(ProductoFilter filter, PaginationFilter pagination);
        Task<ProductoDetalleDto?> ObtenerDetalle(Guid productoId, Guid emisorId);
        Task<Dictionary<Guid, ProductoDto>> ObtenerPorIds(IEnumerable<Guid> ids, Guid emisorId);
    }
}
