using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.DTOs
{
    public sealed class ProductoDto
    {
        public Guid ProductoId { get; set; }
        public string? NombrePublico { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenBase64 { get; set; }
        public bool VisibleEnTienda { get; set; }
        public bool? Destacado { get; set; }
        public bool Exento { get; set; }

        public List<CategoriaDto> Categorias { get; set; } = new();

        // ← NUEVO: cuál de sus categorías coincide con el filtro (si aplica)
        public Guid? CategoriaCoincidenteId { get; set; }
        public string? CategoriaCoincidenteNombre { get; set; }
    }
}
