using EccomerceAPI.Common.Results;
using EccomerceAPI.Contracts.Carrito;

namespace EccomerceAPI.Interfaces
{
    public interface ICarritoService
    {
        Task<ServiceResult<CartSummaryResponse>> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request);
    }
}
