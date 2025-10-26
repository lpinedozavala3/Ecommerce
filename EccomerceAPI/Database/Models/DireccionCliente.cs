using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class DireccionCliente
    {
        public Guid IdDireccion { get; set; }
        public Guid IdCliente { get; set; }
        public string Calle { get; set; } = null!;
        public string? Numero { get; set; }
        public string? Depto { get; set; }
        public string Comuna { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string Pais { get; set; } = null!;
        public string? CodigoPostal { get; set; }
        public string? Referencias { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? ActualizadoEn { get; set; }

        public virtual ClienteTiendum IdClienteNavigation { get; set; } = null!;
    }
}
