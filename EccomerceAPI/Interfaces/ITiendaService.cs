using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;

namespace EccomerceAPI.Interfaces
{
    public interface ITiendaService
    {
        Task<TenantInfoDto?> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default);
    }
}
