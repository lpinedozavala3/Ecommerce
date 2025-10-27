using EccomerceAPI.Contracts.Pedidos;

namespace EccomerceAPI.Interfaces
{
    public interface IPedidosService
    {
        Task<IReadOnlyList<PedidoResumenDto>> ObtenerPedidosAsync(Guid tiendaId, Guid clienteId);
        Task<DireccionClienteDto?> ObtenerDireccionAsync(Guid tiendaId, Guid clienteId);
        Task<DireccionClienteDto> GuardarDireccionAsync(Guid tiendaId, Guid clienteId, UpsertDireccionRequest request);
    }
}
