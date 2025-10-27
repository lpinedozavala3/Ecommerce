using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Database.Models;
using EccomerceAPI.Contracts.Auth;
using EccomerceAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EccomerceAPI.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly contextApp _db;

        public AuthService(contextApp db)
        {
            _db = db;
        }

        public async Task<AuthResponse> RegistrarAsync(RegisterRequest request, Guid tiendaId)
        {
            var email = NormalizarEmail(request.Email);

            var existe = await _db.ClienteTienda
                .AnyAsync(c => c.IdTienda == tiendaId && c.Email == email);

            if (existe)
            {
                throw new InvalidOperationException("El correo ya está registrado en esta tienda.");
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

            return MapearRespuesta(cliente);
        }

        public async Task<AuthResponse> IniciarSesionAsync(LoginRequest request, Guid tiendaId)
        {
            var email = NormalizarEmail(request.Email);
            var cliente = await _db.ClienteTienda
                .FirstOrDefaultAsync(c => c.IdTienda == tiendaId && c.Email == email);

            if (cliente is null)
            {
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            if (cliente.HashPassword is null || !cliente.HashPassword.SequenceEqual(HashPassword(request.Password)))
            {
                throw new InvalidOperationException("Credenciales inválidas.");
            }

            cliente.UltimoLoginEn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return MapearRespuesta(cliente);
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
