using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    [Table("Auditoria")]
    public class Auditory : BaseEntity
    {
        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        [Column("operacion")]
        public string Operacion { get; set; } 
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Column("detalle")]
        public string Detalle { get; set; }

    }
}
