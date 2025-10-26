using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public class CreateUsuarioDto 
    {

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 20 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "El correo es obligatorio.")]
        [StringLength(50, ErrorMessage = "El correo no puede superar los 50 caracteres.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        public string Correo { get; set; }


        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [PasswordPropertyText]
        public string Contraseña { get; set; }


        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(14, ErrorMessage = "El rol no puede superar los 14 caracteres.")]
        [RegularExpression("^(Admin|Recepcionista|Cliente)$",
       ErrorMessage = "El rol debe ser Admin, Recepcionista o Cliente.")]
        public string Rol { get; set; }

    }
}
