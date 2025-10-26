using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EccomerceAPI.Contracts.Carrito
{
    public sealed class CartSummaryRequest
    {
        [Required]
        [MinLength(1)]
        public List<CartItemRequest> Items { get; set; } = new();
    }
}
