using SGHR.Domain.Base;
using SGHR.Domain.Enum.Usuario;

namespace SGHR.Domain.Entities.Configuration.Usuers
{
    public sealed class Usuario : BaseEntity
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
        public EstadoUsuario Estado { get; set; } = EstadoUsuario.Activo;

    }
}
