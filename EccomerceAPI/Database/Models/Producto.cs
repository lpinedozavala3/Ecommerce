using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Producto
    {
        public Producto()
        {
            OrdenItems = new HashSet<OrdenItem>();
            IdCategoria = new HashSet<Categorium>();
        }

        public Guid ProductoId { get; set; }
        public string? CodigoBarra { get; set; }
        public string Nombre { get; set; } = null!;
        public double Precio { get; set; }
        public bool Exento { get; set; }
        public Guid EmisorId { get; set; }
        public bool Activo { get; set; }
        public string? UnidadMedida { get; set; }
        public Guid SucursalId { get; set; }
        public string? NombrePublico { get; set; }
        public string? Slug { get; set; }
        public bool VisibleEnTienda { get; set; }
        public string? DescripcionCorta { get; set; }
        public string? DescripcionLarga { get; set; }
        public string ImagenBase64 { get; set; } = null!;
        public int Stock { get; set; }

        public virtual Emisor Emisor { get; set; } = null!;
        public virtual ICollection<OrdenItem> OrdenItems { get; set; }

        public virtual ICollection<Categorium> IdCategoria { get; set; }
    }
}
