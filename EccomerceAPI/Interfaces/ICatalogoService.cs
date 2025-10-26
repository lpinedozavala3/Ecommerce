using Database.DTOs;
using Database.Filters;

namespace EccomerceAPI.Interfaces
{
    public interface ICatalogoService
    {
        Task<(List<ProductoDto> Items, int Total)> ListFilter(ProductoFilter filter, PaginationFilter pagination);
    }
}
