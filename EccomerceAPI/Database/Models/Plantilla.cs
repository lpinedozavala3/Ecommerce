using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Plantilla
    {
        public Plantilla()
        {
            Tienda = new HashSet<Tiendum>();
        }

        public Guid IdPlantilla { get; set; }
        public string NombrePlantilla { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? Version { get; set; }
        public string TipoEstructura { get; set; } = null!;
        public string? UrlRepositorio { get; set; }
        public string? UrlBundle { get; set; }
        public string? JsonConfigDefecto { get; set; }

        public virtual ICollection<Tiendum> Tienda { get; set; }
    }
}
