using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public PedidosService(contextApp db)
        {
            _db = db;
        }

        public async Task<(bool response, int status, string message, IReadOnlyList<PedidoResumenDto>? data)> ObtenerPedidosAsync(Guid tiendaId, Guid clienteId)
        {
            try
            {
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

                return (true, StatusCodes.Status200OK, "Pedidos obtenidos correctamente.", pedidos);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudieron obtener los pedidos del cliente.", null);
            }
        }

        public async Task<(bool response, int status, string message, DireccionClienteDto? data)> ObtenerDireccionAsync(Guid tiendaId, Guid clienteId)
        {
            try
            {
                var direccion = await _db.DireccionClientes
                    .AsNoTracking()
                    .Where(d => d.IdCliente == clienteId && d.IdClienteNavigation.IdTienda == tiendaId)
                    .Select(DireccionProjection)
                    .FirstOrDefaultAsync();

                if (direccion is null)
                {
                    return (false, StatusCodes.Status404NotFound, "El cliente no tiene una dirección registrada.", null);
                }

                return (true, StatusCodes.Status200OK, "Dirección obtenida correctamente.", direccion);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo obtener la dirección del cliente.", null);
            }
        }

        public async Task<(bool response, int status, string message, DireccionClienteDto? data)> GuardarDireccionAsync(Guid tiendaId, Guid clienteId, UpsertDireccionRequest request)
        {
            try
            {
                var cliente = await _db.ClienteTienda
                    .Include(c => c.DireccionCliente)
                    .FirstOrDefaultAsync(c => c.IdCliente == clienteId && c.IdTienda == tiendaId);

                if (cliente is null)
                {
                    return (false, StatusCodes.Status404NotFound, "No se encontró el cliente para la tienda actual.", null);
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

                return (true, StatusCodes.Status200OK, "Dirección guardada correctamente.", MapDireccion(cliente.DireccionCliente!));
            }
            catch (InvalidOperationException ex)
            {
                return (false, StatusCodes.Status404NotFound, ex.Message, null);
            }
            catch
            {
                return (false, StatusCodes.Status500InternalServerError, "No se pudo guardar la dirección del cliente.", null);
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
