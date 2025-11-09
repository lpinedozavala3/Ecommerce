using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.DTOs;
using EccomerceAPI.Contracts.Carrito;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EccomerceAPI.Services
{
    public sealed class CarritoService : ICarritoService
    {
        private readonly ITenantResolver _tenantResolver;
        private readonly ICatalogoService _catalogoService;
        private const decimal CostoEnvio = 5500m;
        private const decimal TasaIva = 0.19m;

        public CarritoService(ITenantResolver tenantResolver, ICatalogoService catalogoService)
        {
            _tenantResolver = tenantResolver;
            _catalogoService = catalogoService;
        }

        public async Task<Response<CartSummaryResponse>> ObtenerResumenAsync(HttpContext httpContext, CartSummaryRequest request)
        {
            var response = new Response<CartSummaryResponse>();

            if (request?.Items is null || request.Items.Count == 0)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "El carrito está vacío.";
                response.Errors = new[] { response.Message };
                return response;
            }

            try
            {
                var (_, emisorId) = await _tenantResolver.ResolveAsync(httpContext);
                var ids = request.Items.Select(i => i.ProductoId).ToList();

                if (ids.Count == 0)
                {
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Message = "El carrito está vacío.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                var productosResult = await _catalogoService.ObtenerProductosPorIdsAsync(emisorId, ids);
                if (productosResult.Status != StatusCodes.Status200OK || productosResult.Data is null)
                {
                    response.Status = productosResult.Status;
                    response.Message = productosResult.Message ?? "No fue posible obtener los productos del carrito.";
                    response.Errors = productosResult.Errors.Length > 0 ? productosResult.Errors : new[] { response.Message };
                    return response;
                }

                var productos = productosResult.Data;
                var resumen = new CartSummaryResponse();

                foreach (var item in request.Items)
                {
                    if (!productos.TryGetValue(item.ProductoId, out var producto))
                    {
                        resumen.Mensajes.Add($"Producto {item.ProductoId} no disponible.");
                        continue;
                    }

                    var cantidad = Math.Clamp(item.Cantidad, 1, 999);
                    var stockDisponible = producto.Stock;
                    if (stockDisponible <= 0)
                    {
                        resumen.Mensajes.Add($"{producto.NombrePublico ?? "Producto"} no tiene stock disponible.");
                    }

                    if (cantidad > stockDisponible)
                    {
                        cantidad = stockDisponible;
                        resumen.Mensajes.Add($"Se ajustó la cantidad de {producto.NombrePublico ?? "producto"} al stock disponible ({stockDisponible}).");
                    }

                    if (cantidad <= 0)
                    {
                        continue;
                    }

                    var subtotal = producto.Precio * cantidad;
                    var iva = producto.Exento ? 0 : decimal.Round(subtotal * TasaIva, 0, MidpointRounding.AwayFromZero);

                    resumen.Items.Add(new CartItemResponse
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

                    resumen.Subtotal += subtotal;
                    resumen.Impuestos += iva;
                }

                if (resumen.Items.Count == 0)
                {
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Message = "Ninguno de los productos está disponible.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                resumen.Envio = CostoEnvio;

                response.Status = StatusCodes.Status200OK;
                response.Message = "Resumen obtenido correctamente.";
                response.Data = resumen;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener el resumen del carrito." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener el resumen del carrito.";
                return response;
            }
        }
    }
}
