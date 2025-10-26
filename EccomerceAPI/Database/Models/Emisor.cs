using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class Emisor
    {
        public Emisor()
        {
            Categoria = new HashSet<Categorium>();
            DteEmitidos = new HashSet<DteEmitido>();
            Productos = new HashSet<Producto>();
        }

        public Guid EmisorId { get; set; }
        public decimal Rut { get; set; }
        public string Dv { get; set; } = null!;
        public string RazonSocial { get; set; } = null!;
        public string Giro { get; set; } = null!;
        public string DirPart { get; set; } = null!;
        public string DirFact { get; set; } = null!;
        public string CorreoPar { get; set; } = null!;
        public string CorreoFact { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Comuna { get; set; } = null!;
        public int NroResol { get; set; }
        public DateTime FechaResol { get; set; }
        public bool Activo { get; set; }
        public bool EsAgenteRetenedor { get; set; }
        public int Ambiente { get; set; }
        public decimal RepresentanteLegalRut { get; set; }
        public string RepresentanteLegalDv { get; set; } = null!;
        public decimal? Telefono { get; set; }
        public string? PasswordSii { get; set; }
        public Guid? ResellerId { get; set; }
        public bool Deleted { get; set; }
        public string UnidadSii { get; set; } = null!;
        public bool? EnvioAutomaticoCorreos { get; set; }
        public bool DetallesIvaIncluido { get; set; }
        public bool? PasswordSiiVerificada { get; set; }

        public virtual Tiendum? Tiendum { get; set; }
        public virtual ICollection<Categorium> Categoria { get; set; }
        public virtual ICollection<DteEmitido> DteEmitidos { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }
    }
}
