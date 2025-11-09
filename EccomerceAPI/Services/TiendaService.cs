using System;
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

        public async Task<Response<TenantInfoDto>> ObtenerPorNombreFantasiaAsync(string nombreFantasia, CancellationToken cancellationToken = default)
        {
            var response = new Response<TenantInfoDto>();

            if (string.IsNullOrWhiteSpace(nombreFantasia))
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "El nombre de fantasía es obligatorio.";
                response.Errors = new[] { response.Message };
                return response;
            }

            try
            {
                var normalized = nombreFantasia.Trim().ToLowerInvariant();

                var tienda = await _db.Tienda
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

                if (tienda is null)
                {
                    response.Status = StatusCodes.Status404NotFound;
                    response.Message = "No se encontró la tienda solicitada.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                response.Status = StatusCodes.Status200OK;
                response.Message = "Tienda obtenida correctamente.";
                response.Data = tienda;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener la tienda." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener la tienda.";
                return response;
            }
        }
    }
}
