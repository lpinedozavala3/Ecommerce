using Database.DTOs;
using EccomerceAPI.Contracts.Carrito;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EccomerceAPI.Interfaces
{
    public interface ICarritoService
    {
        Task<Response<CartSummaryResponse>> ObtenerResumenAsync(HttpContext httpContext, CartSummaryRequest request);
    }
}
