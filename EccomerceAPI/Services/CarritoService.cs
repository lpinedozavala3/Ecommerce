using System;
using System.Linq;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;

namespace EccomerceAPI.Services
{
    public sealed class CarritoService : ICarritoService
    {
        private readonly ICatalogoService _catalogoService;

        private const decimal CostoEnvio = 5500m;
        private const decimal TasaIva = 0.19m;

        public CarritoService(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public async Task<CartSummaryResponse> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request)
        {
            var ids = request.Items.Select(i => i.ProductoId);
            var productos = await _catalogoService.ObtenerPorIds(ids, emisorId);

            var response = new CartSummaryResponse();

            foreach (var item in request.Items)
            {
                if (!productos.TryGetValue(item.ProductoId, out var producto))
                {
                    response.Mensajes.Add($"Producto {item.ProductoId} no disponible.");
                    continue;
                }

                var cantidad = Math.Clamp(item.Cantidad, 1, 999);
                var stockDisponible = producto.Stock;

                if (stockDisponible <= 0)
                {
                    response.Mensajes.Add($"{producto.NombrePublico ?? "Producto"} no tiene stock disponible.");
                }

                if (cantidad > stockDisponible)
                {
                    cantidad = stockDisponible;
                    response.Mensajes.Add($"Se ajustó la cantidad de {producto.NombrePublico ?? "producto"} al stock disponible ({stockDisponible}).");
                }

                if (cantidad <= 0)
                {
                    continue;
                }

                var subtotal = producto.Precio * cantidad;
                var iva = producto.Exento ? 0 : decimal.Round(subtotal * TasaIva, 0, MidpointRounding.AwayFromZero);

                response.Items.Add(new CartItemResponse
                {
                    ProductoId = producto.ProductoId,
                    Nombre = producto.NombrePublico ?? "Producto",
                    ImagenBase64 = producto.ImagenBase64,
                    Precio = producto.Precio,
                    CantidadSolicitada = cantidad,
                    StockDisponible = stockDisponible,
                    Exento = producto.Exento,
                    Subtotal = subtotal,
                    Iva = iva
                });

                response.Subtotal += subtotal;
                response.Impuestos += iva;
            }

            if (response.Items.Count > 0)
            {
                response.Envio = CostoEnvio;
            }

            return response;
        }
    }
}
