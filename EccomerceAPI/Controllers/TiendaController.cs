using System;
using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EccomerceAPI.Controllers
{
    [ApiController]
    [Route("api/tienda")]
    public class TiendaController : ControllerBase
    {
        private readonly ITiendaService _tiendaService;

        public TiendaController(ITiendaService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        [HttpGet("{nombreFantasia}")]
        [ProducesResponseType(typeof(TenantInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<TenantInfoDto>>> ObtenerPorNombreFantasia(string nombreFantasia, CancellationToken cancellationToken)
        {
            try
            {
                var info = await _tiendaService.ObtenerPorNombreFantasiaAsync(nombreFantasia, cancellationToken);
                if (info is null)
                {
                    var messageTitle = "La tienda no fue encontrada.";
                    var errorMessage = "No existe una tienda con el nombre indicado.";

                    return NotFound(new Response<TenantInfoDto>
                    {
                        Status = StatusCodes.Status404NotFound,
                        Message = messageTitle,
                        Errors = new[] { errorMessage }
                    });
                }

                var messageSuccess = "Información de la tienda recuperada correctamente.";

                return Ok(new Response<TenantInfoDto>
                {
                    Status = StatusCodes.Status200OK,
                    Message = messageSuccess,
                    Data = info
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                var errorMessage = ex.Message;
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador.";
#endif
                var messageTitle = "Error al obtener la información de la tienda.";

                return StatusCode(StatusCodes.Status500InternalServerError, new Response<TenantInfoDto>
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Message = messageTitle,
                    Errors = new[] { errorMessage }
                });
            }
        }
    }
}
