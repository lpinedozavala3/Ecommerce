using EccomerceAPI.Common.Results;
using EccomerceAPI.Contracts.Auth;

namespace EccomerceAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<AuthResponse>> RegistrarAsync(RegisterRequest request, Guid tiendaId);
        Task<ServiceResult<AuthResponse>> IniciarSesionAsync(LoginRequest request, Guid tiendaId);
    }
}
