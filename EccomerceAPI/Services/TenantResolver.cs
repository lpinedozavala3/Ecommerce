using System;
using System.Threading.Tasks;
using EccomerceAPI.Interfaces;

namespace EccomerceAPI.Services
{
    public class TenantResolver : ITenantResolver
    {
        private readonly ITiendaService _tiendaService;

        public TenantResolver(ITiendaService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        public async Task<(Guid tiendaId, Guid emisorId)> ResolveAsync(HttpContext http)
        {
            var storeName = http.Request.Headers["X-Store-Name"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(storeName))
            {
                throw new InvalidOperationException("No se proporcionó el nombre de fantasía de la tienda.");
            }

            storeName = storeName.Trim();

            var tenant = await _tiendaService.ObtenerPorNombreFantasiaAsync(storeName);
            if (tenant is null)
            {
                throw new InvalidOperationException($"La tienda '{storeName}' no está configurada o no se encuentra activa.");
            }

            return (tenant.TiendaId, tenant.EmisorId);
        }
    }
}
