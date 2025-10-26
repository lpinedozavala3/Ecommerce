using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class OrdenEstado
    {
        public OrdenEstado()
        {
            Ordens = new HashSet<Orden>();
        }

        public int IdEstado { get; set; }
        public string NombreEstado { get; set; } = null!;
        public string? Descripcion { get; set; }

        public virtual ICollection<Orden> Ordens { get; set; }
    }
}
