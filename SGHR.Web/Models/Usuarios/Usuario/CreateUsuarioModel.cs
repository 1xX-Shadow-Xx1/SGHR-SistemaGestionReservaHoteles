
using SGHR.Domain.Enum.Usuarios;

namespace SGHR.Web.Models.Usuarios.Usuario
{
    public class CreateUsuarioModel 
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public RolUsuarios Rol { get; set; } = RolUsuarios.Cliente;

    }
}
