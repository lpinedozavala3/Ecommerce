using System.Threading;
using System.Threading.Tasks;
using Database.DTOs;
using EccomerceAPI.Interfaces;
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
        public async Task<ActionResult<Response<TenantInfoDto>>> ObtenerPorNombreFantasia(string nombreFantasia, CancellationToken cancellationToken)
        {
            var response = await _tiendaService.ObtenerPorNombreFantasiaAsync(nombreFantasia, cancellationToken);
            return StatusCode(response.Status, response);
        }
    }
}
