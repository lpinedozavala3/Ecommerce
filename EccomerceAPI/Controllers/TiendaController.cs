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
        [ProducesResponseType(typeof(Response<TenantInfoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<TenantInfoDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response<TenantInfoDto>>> ObtenerPorNombreFantasia(string nombreFantasia, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _tiendaService.ObtenerPorNombreFantasiaAsync(nombreFantasia, cancellationToken);

                if (!result.IsSuccess)
                {
                    var errors = result.Errors.Length > 0 ? result.Errors : new[] { result.Message };
                    return StatusCode((int)result.StatusCode, new Response<TenantInfoDto>
                    {
                        Status = (int)result.StatusCode,
                        Message = string.IsNullOrWhiteSpace(result.Message)
                            ? "No se pudo obtener la tienda."
                            : result.Message,
                        Errors = errors
                    });
                }

                return StatusCode((int)result.StatusCode, new Response<TenantInfoDto>
                {
                    Status = (int)result.StatusCode,
                    Message = string.IsNullOrWhiteSpace(result.Message)
                        ? "Tienda obtenida correctamente."
                        : result.Message,
                    Data = result.Data,
                    Errors = Array.Empty<string>()
                });
            }
            catch (Exception ex)
            {
                var messageTitle = "Error al obtener la tienda";
#if DEBUG
                var errorMessage = ex.Message;
                Console.WriteLine(errorMessage);
#else
                var errorMessage = "Ocurrió un error inesperado. Consulte con el administrador";
#endif

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
