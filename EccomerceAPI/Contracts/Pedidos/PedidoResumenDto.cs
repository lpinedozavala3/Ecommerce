using System;
using System.Collections.Generic;

namespace EccomerceAPI.Contracts.Pedidos
{
    public sealed class PedidoResumenDto
    {
        public Guid OrdenId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public DateTime CreadoEn { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal TotalNeto { get; set; }
        public decimal TotalIva { get; set; }
        public decimal Total { get; set; }
        public IReadOnlyList<PedidoItemDto> Items { get; set; } = Array.Empty<PedidoItemDto>();
    }
}
