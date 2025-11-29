using SGHR.Web.Models.EnumsModel.Usuario;

namespace SGHR.Web.Models.Usuarios.Usuario
{
    public record UpdateUsuarioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public RolUsuarioModel Rol { get; set; }
        public EstadoUsuarioModel Estado { get; set; }
    }
}
