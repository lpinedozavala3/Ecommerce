using EccomerceAPI.Common.Results;
using EccomerceAPI.Contracts.Pedidos;

namespace EccomerceAPI.Interfaces
{
    public interface IPedidosService
    {
        Task<ServiceResult<IReadOnlyList<PedidoResumenDto>>> ObtenerPedidosAsync(Guid tiendaId, Guid clienteId);
        Task<ServiceResult<DireccionClienteDto>> ObtenerDireccionAsync(Guid tiendaId, Guid clienteId);
        Task<ServiceResult<DireccionClienteDto>> GuardarDireccionAsync(Guid tiendaId, Guid clienteId, UpsertDireccionRequest request);
    }
}
