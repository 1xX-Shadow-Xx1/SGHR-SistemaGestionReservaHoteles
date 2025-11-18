using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Users.Cliente
{
    public record UpdateClienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Correo { get; set; }
    }
}
