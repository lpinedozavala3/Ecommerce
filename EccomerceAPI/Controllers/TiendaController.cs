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
        public async Task<IActionResult> ObtenerPorNombreFantasia(string nombreFantasia, CancellationToken cancellationToken)
        {
            var info = await _tiendaService.ObtenerPorNombreFantasiaAsync(nombreFantasia, cancellationToken);
            if (info is null)
            {
                return NotFound();
            }

            return Ok(info);
        }
    }
}
