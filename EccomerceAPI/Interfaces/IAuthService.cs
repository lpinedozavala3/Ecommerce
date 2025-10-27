using EccomerceAPI.Contracts.Auth;

namespace EccomerceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegistrarAsync(RegisterRequest request, Guid tiendaId);
        Task<AuthResponse> IniciarSesionAsync(LoginRequest request, Guid tiendaId);
    }
}
