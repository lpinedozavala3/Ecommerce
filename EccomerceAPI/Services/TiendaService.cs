using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using Database.Models;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
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

        public async Task<(bool response, int status, string message, TenantInfoDto? data)> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nombreFantasia))
            {
                return (false, StatusCodes.Status400BadRequest, "El nombre de fantasía es obligatorio.", null);
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
                    return (false, StatusCodes.Status404NotFound, "La tienda solicitada no existe.", null);
                }

                return (true, StatusCodes.Status200OK, "Tienda obtenida correctamente.", tenant);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo obtener la información de la tienda.", null);
            }
        }
    }
}
