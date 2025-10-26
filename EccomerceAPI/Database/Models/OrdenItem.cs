using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class OrdenItem
    {
        public Guid IdOrdenItem { get; set; }
        public Guid IdOrden { get; set; }
        public Guid IdProducto { get; set; }
        public string NombreItem { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public decimal PrecioNeto { get; set; }
        public decimal Iva { get; set; }
        public bool EsExento { get; set; }

        public virtual Orden IdOrdenNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
