using System;
using System.Collections.Generic;

namespace Database.Models
{
    public partial class DteEmitido
    {
        public DteEmitido()
        {
            Ordens = new HashSet<Orden>();
        }

        public Guid DteId { get; set; }
        public Guid? ReceptorId { get; set; }
        public Guid EmisorId { get; set; }
        public Guid TipoDteId { get; set; }
        public Guid? SucursalId { get; set; }
        public long Folio { get; set; }
        public DateTime Fecha { get; set; }
        public int EstadoSii { get; set; }
        public long? Trackid { get; set; }
        public string? FormaPago { get; set; }
        public Guid? ArchivoExcelId { get; set; }
        public long Neto { get; set; }
        public long Iva { get; set; }
        public double Exento { get; set; }
        public double Total { get; set; }
        public long Impuestos { get; set; }
        public DateTime FechaProcesamiento { get; set; }
        public bool Anulado { get; set; }
        public int CodigoTipoDte { get; set; }
        public string? CodigoTipoDteReferencia { get; set; }
        public string? FolioReferencia { get; set; }
        public string? RazonReferencia { get; set; }
        public int RutEmisor { get; set; }
        public DateTime? FechaReferencia { get; set; }
        public int? RutReceptor { get; set; }
        public Guid? ImportacionMasivaId { get; set; }
        public long TotalImpuestosAdicionales { get; set; }
        public Guid? ProveedorId { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public bool? CedidoCompleto { get; set; }
        public long? IvaTerceros { get; set; }
        public long? IvaPropio { get; set; }
        public int AmbienteId { get; set; }
        public bool ReferenciaGlobal { get; set; }
        public bool Importado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool FolioReutilizado { get; set; }
        /// <summary>
        /// [Description(&quot;Pendiente de Acuse&quot;)]
        ///         Pendiente = 5,
        ///         [Description(&quot;No Reclamado en Plazo&quot;)]
        ///         A = 1,
        ///         [Description(&quot;Forma de Pago Contado&quot;)]
        ///         P = 2, 
        ///         [Description(&quot;Rechazado por el Receptor&quot;)]
        ///         R = 3,
        ///         [Description(&quot;Recibo Otorgado por el Receptor&quot;)]
        ///         C = 4
        /// </summary>
        public int? EstadoAcuse { get; set; }
        public DateTime? FechaUltimaSincronizacion { get; set; }
        public bool Sincronizado { get; set; }
        public Guid? UsuarioId { get; set; }
        public bool Informado { get; set; }
        public string? Observaciones { get; set; }
        public string? Cajero { get; set; }
        public string? Tipopago { get; set; }
        public int? Propina { get; set; }
        public string? RazonSocialReceptor { get; set; }
        public bool? IvaIncluido { get; set; }
        public Guid? OrigenId { get; set; }
        public Guid? IdTipoRechazo { get; set; }
        public bool? IsMarked { get; set; }
        public string? Mediopago { get; set; }
        public bool? DtePagado { get; set; }

        public virtual Emisor Emisor { get; set; } = null!;
        public virtual ICollection<Orden> Ordens { get; set; }
    }
}
