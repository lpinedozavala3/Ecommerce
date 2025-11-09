using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;

namespace EccomerceAPI.Interfaces
{
    public interface ITiendaService
    {
        Task<(bool response, int status, string message, TenantInfoDto? data)> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default);
    }
}
