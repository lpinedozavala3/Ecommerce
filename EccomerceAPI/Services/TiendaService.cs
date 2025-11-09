using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using Database.Models;
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

        public async Task<TenantInfoDto?> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(nombreFantasia))
            {
                return null;
            }

            var normalized = nombreFantasia.Trim().ToLowerInvariant();

            return await _db.Tienda
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
        }
    }
}
