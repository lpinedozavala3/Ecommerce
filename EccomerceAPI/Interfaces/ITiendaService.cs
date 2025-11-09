using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using EccomerceAPI.Common.Results;

namespace EccomerceAPI.Interfaces
{
    public interface ITiendaService
    {
        Task<ServiceResult<TenantInfoDto>> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default);
    }
}
