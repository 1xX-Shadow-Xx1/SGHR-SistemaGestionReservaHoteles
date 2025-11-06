
using SGHR.Domain.Enum.Usuario;
using SGHR.Domain.Enum.Usuarios;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public record UpdateUsuarioDto
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }
        [StringLength(50)]
        [EmailAddress]
        public string Correo { get; set; }
        [PasswordPropertyText]
        public string Contraseña { get; set; }
        public RolUsuarios Rol { get; set; }
        public EstadoUsuario Estado { get; set; }
    }
}
