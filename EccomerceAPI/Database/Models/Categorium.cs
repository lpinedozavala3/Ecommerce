using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Categorium
    {
        public Categorium()
        {
            InverseIdCategoriaPadreNavigation = new HashSet<Categorium>();
            IdProductos = new HashSet<Producto>();
        }

        public Guid IdCategoria { get; set; }
        public Guid IdEmisor { get; set; }
        public string NombreCategoria { get; set; } = null!;
        public string? SlugCategoria { get; set; }
        public Guid? IdCategoriaPadre { get; set; }

        public virtual Categorium? IdCategoriaPadreNavigation { get; set; }
        public virtual Emisor IdEmisorNavigation { get; set; } = null!;
        public virtual ICollection<Categorium> InverseIdCategoriaPadreNavigation { get; set; }

        public virtual ICollection<Producto> IdProductos { get; set; }
    }
}
