using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public class UsuarioCreateDto 
    {
        
        [Required]      
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Contraseña { get; set; }
        [Required]
        [StringLength(14)]
        [RegularExpression("^(Admin|Recepcionista|Cliente)$", ErrorMessage = "El rol debe ser Admin, Recepcionista o Cliente.")]
        public string Rol { get; set; }

    }
}
