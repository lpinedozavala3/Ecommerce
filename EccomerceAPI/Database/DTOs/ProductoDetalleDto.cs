using System;
using System.Collections.Generic;

namespace Database.DTOs
{
    public sealed class ProductoDetalleDto : ProductoDto
    {
        public string? DescripcionCorta { get; set; }
        public string? DescripcionLarga { get; set; }
        public string? CodigoBarra { get; set; }
        public string? UnidadMedida { get; set; }
        public bool Activo { get; set; }
    }
}
