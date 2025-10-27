using System.ComponentModel.DataAnnotations;

namespace EccomerceAPI.Contracts.Pedidos
{
    public sealed class UpsertDireccionRequest
    {
        [Required]
        [MaxLength(200)]
        public string Calle { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Numero { get; set; }

        [MaxLength(100)]
        public string? Depto { get; set; }

        [Required]
        [MaxLength(120)]
        public string Comuna { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Ciudad { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Region { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Pais { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CodigoPostal { get; set; }

        [MaxLength(250)]
        public string? Referencias { get; set; }
    }
}
