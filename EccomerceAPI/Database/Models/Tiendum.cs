using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Tiendum
    {
        public Tiendum()
        {
            ClienteTienda = new HashSet<ClienteTiendum>();
            Dominios = new HashSet<Dominio>();
            Ordens = new HashSet<Orden>();
        }

        public Guid IdTienda { get; set; }
        public Guid IdEmisor { get; set; }
        public string NombreComercial { get; set; } = null!;
        public string EstadoTienda { get; set; } = null!;
        public Guid IdPlantilla { get; set; }
        public string? JsonBranding { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }
        public string? NombreFantasia { get; set; }

        public virtual Emisor IdEmisorNavigation { get; set; } = null!;
        public virtual Plantilla IdPlantillaNavigation { get; set; } = null!;
        public virtual ICollection<ClienteTiendum> ClienteTienda { get; set; }
        public virtual ICollection<Dominio> Dominios { get; set; }
        public virtual ICollection<Orden> Ordens { get; set; }
    }
}
