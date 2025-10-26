using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Dominio
    {
        public Guid IdDominio { get; set; }
        public Guid IdTienda { get; set; }
        public string TipoDominio { get; set; } = null!;
        public string ValorDominio { get; set; } = null!;
        public string EstadoVerificacion { get; set; } = null!;
        public DateTime CreadoEn { get; set; }
        public DateTime ActualizadoEn { get; set; }

        public virtual Tiendum IdTiendaNavigation { get; set; } = null!;
    }
}
