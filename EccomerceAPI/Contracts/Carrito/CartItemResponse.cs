using System;

namespace EccomerceAPI.Contracts.Carrito
{
    public sealed class CartItemResponse
    {
        public Guid ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? ImagenBase64 { get; set; }
        public decimal Precio { get; set; }
        public int CantidadSolicitada { get; set; }
        public int StockDisponible { get; set; }
        public bool Exento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public bool SinStock => StockDisponible <= 0;
    }
}
