using System;
using System.Collections.Generic;
using System.Linq;
using Database.DTOs;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EccomerceAPI.Services
{
    public class CarritoService : ICarritoService
    {
        private const decimal CostoEnvio = 5500m;
        private const decimal TasaIva = 0.19m;

        private readonly ICatalogoService _catalogoService;

        public CarritoService(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        public async Task<(bool response, int status, string message, CartSummaryResponse? data)> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request)
        {
            if (request?.Items is null || request.Items.Count == 0)
            {
                return (false, StatusCodes.Status400BadRequest, "El carrito está vacío.", null);
            }

            try
            {
                var ids = request.Items.Select(i => i.ProductoId);
                var productosResult = await _catalogoService.ObtenerPorIdsAsync(ids, emisorId);
                if (!productosResult.response)
                {
                    return (false, productosResult.status, string.IsNullOrWhiteSpace(productosResult.message)
                        ? "No se pudieron obtener los productos del catálogo."
                        : productosResult.message, null);
                }

                var productos = productosResult.data ?? new Dictionary<Guid, ProductoDto>();
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
                        continue;
                    }

                    if (cantidad > stockDisponible)
                    {
                        cantidad = stockDisponible;
                        response.Mensajes.Add($"Se ajustó la cantidad de {producto.NombrePublico ?? "producto"} al stock disponible ({stockDisponible}).");
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

                if (response.Items.Count == 0)
                {
                    return (false, StatusCodes.Status400BadRequest, "Ninguno de los productos está disponible.", null);
                }

                response.Envio = CostoEnvio;
                return (true, StatusCodes.Status200OK, "Resumen del carrito calculado correctamente.", response);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo calcular el resumen del carrito.", null);
            }
        }
    }
}
