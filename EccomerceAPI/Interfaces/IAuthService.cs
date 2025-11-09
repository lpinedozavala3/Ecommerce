using EccomerceAPI.Contracts.Auth;

namespace EccomerceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<(bool response, int status, string message, AuthResponse? data)> RegistrarAsync(RegisterRequest request, Guid tiendaId);
        Task<(bool response, int status, string message, AuthResponse? data)> IniciarSesionAsync(LoginRequest request, Guid tiendaId);
    }
}
