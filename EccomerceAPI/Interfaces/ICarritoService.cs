using EccomerceAPI.Contracts.Carrito;

namespace EccomerceAPI.Interfaces
{
    public interface ICarritoService
    {
        Task<CartSummaryResponse> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request);
    }
}
