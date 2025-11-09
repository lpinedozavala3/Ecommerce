using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Database.DTOs;
using Database.Models;
using EccomerceAPI.Contracts.Pedidos;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Services
{
    public sealed class PedidosService : IPedidosService
    {
        private readonly contextApp _db;
        private readonly ITenantResolver _tenantResolver;
        private static readonly Expression<Func<DireccionCliente, DireccionClienteDto>> DireccionProjection =
            d => new DireccionClienteDto
            {
                IdDireccion = d.IdDireccion,
                ClienteId = d.IdCliente,
                Calle = d.Calle,
                Numero = d.Numero,
                Depto = d.Depto,
                Comuna = d.Comuna,
                Ciudad = d.Ciudad,
                Region = d.Region,
                Pais = d.Pais,
                CodigoPostal = d.CodigoPostal,
                Referencias = d.Referencias,
                EsPrincipal = d.EsPrincipal,
                CreadoEn = d.CreadoEn,
                ActualizadoEn = d.ActualizadoEn
            };

        public PedidosService(contextApp db, ITenantResolver tenantResolver)
        {
            _db = db;
            _tenantResolver = tenantResolver;
        }

        public async Task<Response<IReadOnlyList<PedidoResumenDto>>> ObtenerPedidosAsync(HttpContext httpContext, Guid clienteId)
        {
            var response = new Response<IReadOnlyList<PedidoResumenDto>>();

            if (clienteId == Guid.Empty)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "El identificador del cliente es obligatorio.";
                response.Errors = new[] { response.Message };
                return response;
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(httpContext);
                var pedidos = await _db.Ordens
                    .AsNoTracking()
                    .Where(o => o.IdTienda == tiendaId && o.IdCliente == clienteId)
                    .OrderByDescending(o => o.CreadoEn)
                    .Select(o => new PedidoResumenDto
                    {
                        OrdenId = o.IdOrden,
                        Codigo = o.IdOrden.ToString("N").Substring(0, 8).ToUpperInvariant(),
                        CreadoEn = o.CreadoEn,
                        Estado = o.IdEstadoNavigation.NombreEstado,
                        TotalNeto = o.TotalNeto,
                        TotalIva = o.TotalIva,
                        Total = o.TotalBruto,
                        Items = o.OrdenItems.Select(i => new PedidoItemDto
                        {
                            OrdenItemId = i.IdOrdenItem,
                            OrdenId = i.IdOrden,
                            ProductoId = i.IdProducto,
                            Nombre = i.NombreItem,
                            Cantidad = i.Cantidad,
                            PrecioNeto = i.PrecioNeto,
                            Iva = i.Iva,
                            EsExento = i.EsExento
                        }).ToList()
                    })
                    .ToListAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = pedidos.Count > 0 ? "Pedidos obtenidos correctamente." : "El cliente no registra pedidos.";
                response.Data = pedidos;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener los pedidos." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener los pedidos.";
                return response;
            }
        }

        public async Task<Response<DireccionClienteDto>> ObtenerDireccionAsync(HttpContext httpContext, Guid clienteId)
        {
            var response = new Response<DireccionClienteDto>();

            if (clienteId == Guid.Empty)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "El identificador del cliente es obligatorio.";
                response.Errors = new[] { response.Message };
                return response;
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(httpContext);
                var direccion = await _db.DireccionClientes
                    .AsNoTracking()
                    .Where(d => d.IdCliente == clienteId && d.IdClienteNavigation.IdTienda == tiendaId)
                    .Select(DireccionProjection)
                    .FirstOrDefaultAsync();

                if (direccion is null)
                {
                    response.Status = StatusCodes.Status204NoContent;
                    response.Message = "El cliente no registra una dirección.";
                    response.Data = null;
                    return response;
                }

                response.Status = StatusCodes.Status200OK;
                response.Message = "Dirección obtenida correctamente.";
                response.Data = direccion;
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al obtener la dirección." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al obtener la dirección.";
                return response;
            }
        }

        public async Task<Response<DireccionClienteDto>> GuardarDireccionAsync(HttpContext httpContext, Guid clienteId, UpsertDireccionRequest request)
        {
            var response = new Response<DireccionClienteDto>();

            if (clienteId == Guid.Empty)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "El identificador del cliente es obligatorio.";
                response.Errors = new[] { response.Message };
                return response;
            }

            if (request is null)
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "Los datos de la dirección son obligatorios.";
                response.Errors = new[] { response.Message };
                return response;
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);
            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                response.Status = StatusCodes.Status400BadRequest;
                response.Message = "Los datos proporcionados no son válidos.";
                response.Errors = validationResults.Select(r => r.ErrorMessage ?? string.Empty).ToArray();
                return response;
            }

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(httpContext);
                var cliente = await _db.ClienteTienda
                    .Include(c => c.DireccionCliente)
                    .FirstOrDefaultAsync(c => c.IdCliente == clienteId && c.IdTienda == tiendaId);

                if (cliente is null)
                {
                    response.Status = StatusCodes.Status404NotFound;
                    response.Message = "No se encontró el cliente para la tienda actual.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                var ahora = DateTime.UtcNow;

                if (cliente.DireccionCliente is null)
                {
                    var direccion = new DireccionCliente
                    {
                        IdDireccion = Guid.NewGuid(),
                        IdCliente = clienteId,
                        Calle = request.Calle.Trim(),
                        Numero = request.Numero?.Trim(),
                        Depto = request.Depto?.Trim(),
                        Comuna = request.Comuna.Trim(),
                        Ciudad = request.Ciudad.Trim(),
                        Region = request.Region.Trim(),
                        Pais = request.Pais.Trim(),
                        CodigoPostal = request.CodigoPostal?.Trim(),
                        Referencias = request.Referencias?.Trim(),
                        EsPrincipal = true,
                        CreadoEn = ahora,
                        ActualizadoEn = ahora
                    };

                    cliente.DireccionCliente = direccion;
                    _db.DireccionClientes.Add(direccion);
                }
                else
                {
                    var direccion = cliente.DireccionCliente;
                    direccion.Calle = request.Calle.Trim();
                    direccion.Numero = request.Numero?.Trim();
                    direccion.Depto = request.Depto?.Trim();
                    direccion.Comuna = request.Comuna.Trim();
                    direccion.Ciudad = request.Ciudad.Trim();
                    direccion.Region = request.Region.Trim();
                    direccion.Pais = request.Pais.Trim();
                    direccion.CodigoPostal = request.CodigoPostal?.Trim();
                    direccion.Referencias = request.Referencias?.Trim();
                    direccion.EsPrincipal = true;
                    direccion.ActualizadoEn = ahora;
                }

                await _db.SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = "Dirección guardada correctamente.";
                response.Data = MapDireccion(cliente.DireccionCliente!);
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error al guardar la dirección." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al guardar la dirección.";
                return response;
            }
        }

        private static DireccionClienteDto MapDireccion(DireccionCliente direccion)
        {
            return new DireccionClienteDto
            {
                IdDireccion = direccion.IdDireccion,
                ClienteId = direccion.IdCliente,
                Calle = direccion.Calle,
                Numero = direccion.Numero,
                Depto = direccion.Depto,
                Comuna = direccion.Comuna,
                Ciudad = direccion.Ciudad,
                Region = direccion.Region,
                Pais = direccion.Pais,
                CodigoPostal = direccion.CodigoPostal,
                Referencias = direccion.Referencias,
                EsPrincipal = direccion.EsPrincipal,
                CreadoEn = direccion.CreadoEn,
                ActualizadoEn = direccion.ActualizadoEn
            };
        }
    }
}
