using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Sesiones
{
    [Table("Sesiones")]
    public class Sesion : BaseEntity 
    {
        [Column("UsuarioID")]
        public int UsuarioID { get; set; }
        [Column("estado")]
        public bool Estado { get; set; }
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }
    }
}
