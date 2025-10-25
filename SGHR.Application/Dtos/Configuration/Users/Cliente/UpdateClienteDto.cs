using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Users.Cliente
{
    public record UpdateClienteDto
    {
        [Required(ErrorMessage = "El id es obligatorio.")]
        public int Id { get; set; }


        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres.")]
        public string Nombre { get; set; }


        [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres.")]
        public string Apellido { get; set; }

                
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$", ErrorMessage = "La cédula debe tener el formato 000-0000000-0.")]
        public string Cedula { get; set; }


        [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "El teléfono solo puede contener números y un posible prefijo '+', con entre 8 y 15 dígitos.")]
        public string? Telefono { get; set; }


        [StringLength(255, ErrorMessage = "La dirección no puede superar los 255 caracteres.")]
        public string? Direccion { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string? Correo { get; set; }
    }
}
