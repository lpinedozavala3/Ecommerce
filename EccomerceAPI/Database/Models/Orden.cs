using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Orden
    {
        public Orden()
        {
            OrdenItems = new HashSet<OrdenItem>();
        }

        public Guid IdOrden { get; set; }
        public Guid IdTienda { get; set; }
        public Guid IdCliente { get; set; }
        public int IdEstado { get; set; }
        public decimal TotalNeto { get; set; }
        public decimal TotalIva { get; set; }
        public decimal TotalBruto { get; set; }
        public Guid? IdDte { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }

        public virtual ClienteTiendum IdClienteNavigation { get; set; } = null!;
        public virtual DteEmitido? IdDteNavigation { get; set; }
        public virtual OrdenEstado IdEstadoNavigation { get; set; } = null!;
        public virtual Tiendum IdTiendaNavigation { get; set; } = null!;
        public virtual ICollection<OrdenItem> OrdenItems { get; set; }
    }
}
