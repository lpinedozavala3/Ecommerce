using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class ClienteTiendum
    {
        public ClienteTiendum()
        {
            Ordens = new HashSet<Orden>();
        }

        public Guid IdCliente { get; set; }
        public Guid IdTienda { get; set; }
        public string Email { get; set; } = null!;
        public byte[]? HashPassword { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? UltimoLoginEn { get; set; }

        public virtual Tiendum IdTiendaNavigation { get; set; } = null!;
        public virtual DireccionCliente? DireccionCliente { get; set; }
        public virtual ICollection<Orden> Ordens { get; set; }
    }
}
