using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTOs
{
    public class TenantInfoDto
    {
        public Guid TiendaId { get; set; }
        public Guid EmisorId { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string? Plantilla { get; set; }         // NombrePlantilla o TipoEstructura
        public string? BrandingJson { get; set; }      // JsonBranding
    }
}
