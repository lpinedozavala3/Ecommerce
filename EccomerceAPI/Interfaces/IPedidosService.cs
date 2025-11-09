using EccomerceAPI.Contracts.Pedidos;

namespace EccomerceAPI.Interfaces
{
    public interface IPedidosService
    {
        Task<(bool response, int status, string message, IReadOnlyList<PedidoResumenDto>? data)> ObtenerPedidosAsync(Guid tiendaId, Guid clienteId);
        Task<(bool response, int status, string message, DireccionClienteDto? data)> ObtenerDireccionAsync(Guid tiendaId, Guid clienteId);
        Task<(bool response, int status, string message, DireccionClienteDto? data)> GuardarDireccionAsync(Guid tiendaId, Guid clienteId, UpsertDireccionRequest request);
    }
}
