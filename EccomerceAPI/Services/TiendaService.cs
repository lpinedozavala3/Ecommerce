using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using Database.Models;
using EccomerceAPI.Common.Results;
using EccomerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Services
{
    public class TiendaService : ITiendaService
    {
        private readonly contextApp _db;

        public TiendaService(contextApp db)
        {
            _db = db;
        }

        public async Task<ServiceResult<TenantInfoDto>> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nombreFantasia))
            {
                return ServiceResult<TenantInfoDto>.Failure("El nombre de fantasía es obligatorio.", HttpStatusCode.BadRequest);
            }

            try
            {
                var normalized = nombreFantasia.Trim().ToLowerInvariant();

                var tenant = await _db.Tienda
                    .AsNoTracking()
                    .Where(t => t.NombreFantasia != null && t.NombreFantasia.ToLower() == normalized && t.EstadoTienda == "ACTIVA")
                    .Select(t => new TenantInfoDto
                    {
                        TiendaId = t.IdTienda,
                        EmisorId = t.IdEmisor,
                        NombreComercial = t.NombreComercial ?? string.Empty,
                        NombreFantasia = t.NombreFantasia!,
                        Plantilla = t.IdPlantillaNavigation.TipoEstructura,
                        BrandingJson = t.JsonBranding
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (tenant is null)
                {
                    return ServiceResult<TenantInfoDto>.Failure("La tienda solicitada no existe.", HttpStatusCode.NotFound);
                }

                return ServiceResult<TenantInfoDto>.Success(
                    tenant,
                    HttpStatusCode.OK,
                    "Tienda obtenida correctamente.");
            }
            catch
            {
                return ServiceResult<TenantInfoDto>.Failure(
                    "No se pudo obtener la información de la tienda.",
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}
