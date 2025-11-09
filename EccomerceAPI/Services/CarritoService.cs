using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Database.DTOs;
using EccomerceAPI.Common.Results;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;

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

        public async Task<ServiceResult<CartSummaryResponse>> ObtenerResumenAsync(Guid emisorId, CartSummaryRequest request)
        {
            if (request?.Items is null || request.Items.Count == 0)
            {
                return ServiceResult<CartSummaryResponse>.Failure(
                    "El carrito está vacío.",
                    HttpStatusCode.BadRequest);
            }

            try
            {
                var ids = request.Items.Select(i => i.ProductoId);
                var productosResult = await _catalogoService.ObtenerPorIdsAsync(ids, emisorId);
                if (!productosResult.IsSuccess)
                {
                    return ServiceResult<CartSummaryResponse>.Failure(
                        string.IsNullOrWhiteSpace(productosResult.Message)
                            ? "No se pudieron obtener los productos del catálogo."
                            : productosResult.Message,
                        productosResult.StatusCode,
                        productosResult.Errors);
                }

                var productos = productosResult.Data ?? new Dictionary<Guid, ProductoDto>();
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
                    return ServiceResult<CartSummaryResponse>.Failure(
                        "Ninguno de los productos está disponible.",
                        HttpStatusCode.BadRequest);
                }

                response.Envio = CostoEnvio;
                return ServiceResult<CartSummaryResponse>.Success(
                    response,
                    HttpStatusCode.OK,
                    "Resumen del carrito calculado correctamente.");
            }
            catch
            {
                return ServiceResult<CartSummaryResponse>.Failure(
                    "No se pudo calcular el resumen del carrito.",
                    HttpStatusCode.InternalServerError);
            }
        }
    }
}
