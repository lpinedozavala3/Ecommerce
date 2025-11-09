using EccomerceAPI.Contracts.Carrito;

namespace EccomerceAPI.Interfaces
{
    public interface ICarritoService
    {
        Task<(bool response, int status, string message, CartSummaryResponse? data)> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request);
    }
}
