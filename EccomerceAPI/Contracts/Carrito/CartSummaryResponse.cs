using System.Collections.Generic;

namespace EccomerceAPI.Contracts.Carrito
{
    public sealed class CartSummaryResponse
    {
        public List<CartItemResponse> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Envio { get; set; }
        public decimal Total => Subtotal + Impuestos + Envio;
        public List<string> Mensajes { get; set; } = new();
    }
}
