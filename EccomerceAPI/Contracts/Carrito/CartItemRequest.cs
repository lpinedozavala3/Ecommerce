using System;
using System.ComponentModel.DataAnnotations;

namespace EccomerceAPI.Contracts.Carrito
{
    public sealed class CartItemRequest
    {
        [Required]
        public Guid ProductoId { get; set; }

        [Range(1, 999)]
        public int Cantidad { get; set; } = 1;
    }
}
