
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public record UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public RolUsuarios Rol { get; set; }
        public EstadoUsuario Estado { get; set; }
    }
}
