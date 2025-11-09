using System;
using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EccomerceAPI.Interfaces;

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

                if (!result.response)
                {
                    var status = result.status;
                    var message = string.IsNullOrWhiteSpace(result.message)
                        ? "No se pudo obtener la tienda."
                        : result.message;

                    if (status == StatusCodes.Status500InternalServerError)
                    {
                        return StatusCode(status, new Response<TenantInfoDto>
                        {
                            Status = status,
                            Message = message,
                            Errors = new[] { message }
                        });
                    }

                    return StatusCode(status, new Response<TenantInfoDto>
                    {
                        Status = status,
                        Message = message,
                        Errors = new[] { message }
                    });
                }

                return StatusCode(result.status, new Response<TenantInfoDto>
                {
                    Status = result.status,
                    Message = string.IsNullOrWhiteSpace(result.message)
                        ? "Tienda obtenida correctamente."
                        : result.message,
                    Data = result.data,
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
