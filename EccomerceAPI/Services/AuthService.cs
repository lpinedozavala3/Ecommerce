using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Database.DTOs;
using Database.Models;
using EccomerceAPI.Contracts.Auth;
using EccomerceAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EccomerceAPI.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly contextApp _db;
        private readonly ITenantResolver _tenantResolver;

        public AuthService(contextApp db, ITenantResolver tenantResolver)
        {
            _db = db;
            _tenantResolver = tenantResolver;
        }

        public async Task<Response<AuthResponse>> RegistrarAsync(HttpContext httpContext, RegisterRequest request)
        {
            var response = new Response<AuthResponse>();

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(httpContext);
                var email = NormalizarEmail(request.Email);

                var existe = await _db.ClienteTienda
                    .AnyAsync(c => c.IdTienda == tiendaId && c.Email == email);

                if (existe)
                {
                    response.Status = StatusCodes.Status409Conflict;
                    response.Message = "El correo ya está registrado en esta tienda.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                var cliente = new ClienteTiendum
                {
                    IdCliente = Guid.NewGuid(),
                    IdTienda = tiendaId,
                    Email = email,
                    HashPassword = HashPassword(request.Password),
                    Nombre = request.Nombre,
                    Apellido = request.Apellido,
                    Telefono = string.IsNullOrWhiteSpace(request.Telefono) ? null : request.Telefono,
                    CreadoEn = DateTime.UtcNow,
                    UltimoLoginEn = DateTime.UtcNow
                };

                _db.ClienteTienda.Add(cliente);
                await _db.SaveChangesAsync();

                response.Status = StatusCodes.Status201Created;
                response.Message = "Usuario registrado correctamente.";
                response.Data = MapearRespuesta(cliente);
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error inesperado. Consulte con el administrador." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al registrar el usuario.";
                return response;
            }
        }

        public async Task<Response<AuthResponse>> IniciarSesionAsync(HttpContext httpContext, LoginRequest request)
        {
            var response = new Response<AuthResponse>();

            try
            {
                var (tiendaId, _) = await _tenantResolver.ResolveAsync(httpContext);
                var email = NormalizarEmail(request.Email);
                var cliente = await _db.ClienteTienda
                    .FirstOrDefaultAsync(c => c.IdTienda == tiendaId && c.Email == email);

                if (cliente is null || cliente.HashPassword is null || !cliente.HashPassword.SequenceEqual(HashPassword(request.Password)))
                {
                    response.Status = StatusCodes.Status401Unauthorized;
                    response.Message = "Credenciales inválidas.";
                    response.Errors = new[] { response.Message };
                    return response;
                }

                cliente.UltimoLoginEn = DateTime.UtcNow;
                await _db.SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = "Inicio de sesión exitoso.";
                response.Data = MapearRespuesta(cliente);
                return response;
            }
            catch (Exception ex)
            {
#if DEBUG
                response.Errors = new[] { ex.Message };
#else
                response.Errors = new[] { "Ocurrió un error inesperado. Consulte con el administrador." };
#endif
                response.Status = StatusCodes.Status500InternalServerError;
                response.Message = "Error al iniciar sesión.";
                return response;
            }
        }

        private static string NormalizarEmail(string email)
            => email.Trim().ToLowerInvariant();

        private static byte[] HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static AuthResponse MapearRespuesta(ClienteTiendum cliente)
        {
            return new AuthResponse
            {
                ClienteId = cliente.IdCliente,
                TiendaId = cliente.IdTienda,
                Email = cliente.Email,
                Nombre = cliente.Nombre ?? string.Empty,
                Apellido = cliente.Apellido ?? string.Empty,
                Telefono = cliente.Telefono,
                EmitidoEn = DateTime.UtcNow,
                Token = GenerarToken()
            };
        }

        private static string GenerarToken()
        {
            var buffer = new byte[32];
            RandomNumberGenerator.Fill(buffer);
            return Convert.ToBase64String(buffer);
        }
    }
}
