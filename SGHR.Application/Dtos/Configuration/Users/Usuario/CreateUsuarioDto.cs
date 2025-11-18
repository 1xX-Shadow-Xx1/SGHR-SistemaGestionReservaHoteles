using SGHR.Domain.Enum.Usuarios;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public class CreateUsuarioDto 
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public RolUsuarios Rol { get; set; }

    }
}
