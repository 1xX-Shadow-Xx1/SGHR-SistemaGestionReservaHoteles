
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
        [StringLength(14)]
        [RegularExpression("^(Admin|Recepcionista|Cliente)$", ErrorMessage = "El rol debe ser Admin, Recepcionista o Cliente.")]
        public string Rol { get; set; }
        [StringLength(14)]
        [RegularExpression("^(Activo|Inactivo|Suspendido|Eliminado)$", ErrorMessage = "El rol debe ser Activo, Inactivo, Suspendido o Cliente.")]
        public string Estado { get; set; }
    }
}
