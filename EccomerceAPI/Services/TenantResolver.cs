using Database.Models;
using EccomerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Services
{
    public class TenantResolver : ITenantResolver
    {
        private readonly contextApp _db; // <-- tu DbContext scaffoldeado
        public TenantResolver(contextApp db) => _db = db;

        public async Task<(Guid tiendaId, Guid emisorId)> ResolveAsync(HttpContext http)
        {
            // 1) Lee el dominio desde el header (front) o, si no viene, del host de la request
            var domain = http.Request.Headers["X-Store-Domain"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(domain))
                domain = http.Request.Host.Host;

            domain = domain?.ToLowerInvariant();

            // 2) ecommerce.DOMINIO -> TIENDA -> EMISOR
            var q = from d in _db.Dominios                              // Dominio
                    join t in _db.Tienda on d.IdTienda equals t.IdTienda // Tiendum
                    join e in _db.Emisors on t.IdEmisor equals e.EmisorId
                    where d.ValorDominio.ToLower() == domain
                          && (d.EstadoVerificacion == "VERIFICADO" || d.EstadoVerificacion == "PENDIENTE")
                    select new { t.IdTienda, e.EmisorId };

            var result = await q.AsNoTracking().FirstOrDefaultAsync();
            if (result is null)
            {
                throw new InvalidOperationException(
                    $"Dominio '{domain}' no está configurado en ecommerce.DOMINIO.");
            }

            return (result.IdTienda, result.EmisorId);
        }
    }
}
