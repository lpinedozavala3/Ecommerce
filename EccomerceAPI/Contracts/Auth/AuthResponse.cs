using System;

namespace EccomerceAPI.Contracts.Auth
{
    public sealed class AuthResponse
    {
        public Guid ClienteId { get; set; }
        public Guid TiendaId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime EmitidoEn { get; set; }
    }
}
