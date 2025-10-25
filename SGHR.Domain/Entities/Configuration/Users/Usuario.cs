using SGHR.Domain.Base;
using SGHR.Domain.Enum.Usuario;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Usuers
{
    [Table("Usuarios")]
    public sealed class Usuario : BaseEntity
    {
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("correo")]
        public string Correo { get; set; }
        [Column("contraseña")]
        public string Contraseña { get; set; }
        [Column("rol")]
        public string Rol { get; set; }
        [Column("estado")]
        public EstadoUsuario Estado { get; set; } = EstadoUsuario.Activo;

    }
}
