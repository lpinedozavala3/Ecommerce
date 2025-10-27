using System.ComponentModel.DataAnnotations;

namespace EccomerceAPI.Contracts.Auth
{
    public sealed class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(120)]
        public string Apellido { get; set; } = string.Empty;

        [Phone]
        [MaxLength(50)]
        public string? Telefono { get; set; }
    }
}
