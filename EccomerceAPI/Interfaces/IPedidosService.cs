using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.DTOs;
using EccomerceAPI.Contracts.Pedidos;
using Microsoft.AspNetCore.Http;

namespace EccomerceAPI.Interfaces
{
    public interface IPedidosService
    {
        Task<Response<IReadOnlyList<PedidoResumenDto>>> ObtenerPedidosAsync(HttpContext httpContext, Guid clienteId);
        Task<Response<DireccionClienteDto>> ObtenerDireccionAsync(HttpContext httpContext, Guid clienteId);
        Task<Response<DireccionClienteDto>> GuardarDireccionAsync(HttpContext httpContext, Guid clienteId, UpsertDireccionRequest request);
    }
}
