using System;

namespace EccomerceAPI.Contracts.Pedidos
{
    public sealed class PedidoItemDto
    {
        public Guid OrdenItemId { get; set; }
        public Guid OrdenId { get; set; }
        public Guid ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public decimal PrecioNeto { get; set; }
        public decimal Iva { get; set; }
        public bool EsExento { get; set; }

        public decimal Subtotal => decimal.Round(Cantidad * PrecioNeto, 0, MidpointRounding.AwayFromZero);
    }
}
