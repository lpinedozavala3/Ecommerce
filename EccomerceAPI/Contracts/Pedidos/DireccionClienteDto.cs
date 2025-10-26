using System;

namespace EccomerceAPI.Contracts.Pedidos
{
    public sealed class DireccionClienteDto
    {
        public Guid IdDireccion { get; set; }
        public Guid ClienteId { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string? Numero { get; set; }
        public string? Depto { get; set; }
        public string Comuna { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string? CodigoPostal { get; set; }
        public string? Referencias { get; set; }
        public bool EsPrincipal { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime? ActualizadoEn { get; set; }
    }
}
