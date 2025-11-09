using Database.DTOs;
using EccomerceAPI.Contracts.Auth;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EccomerceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<Response<AuthResponse>> RegistrarAsync(HttpContext httpContext, RegisterRequest request);
        Task<Response<AuthResponse>> IniciarSesionAsync(HttpContext httpContext, LoginRequest request);
    }
}
