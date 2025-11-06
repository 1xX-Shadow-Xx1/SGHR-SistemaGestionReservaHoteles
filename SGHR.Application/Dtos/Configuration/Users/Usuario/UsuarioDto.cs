using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public class UsuarioDto 
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public RolUsuarios Rol { get; set; } 
        public EstadoUsuario Estado { get; set; } 
    }
}
