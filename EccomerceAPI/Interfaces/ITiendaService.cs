using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;

namespace EccomerceAPI.Interfaces
{
    public interface ITiendaService
    {
        Task<Response<TenantInfoDto>> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default);
    }
}
