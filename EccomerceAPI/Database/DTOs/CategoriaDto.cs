using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTOs
{
    public class CategoriaDto
    {
        public Guid IdCategoria { get; set; }
        public string NombreCategoria { get; set; } = string.Empty;
        public string? SlugCategoria { get; set; }
        public int? ProductosVisibles { get; set; } // opcional
    }
}
