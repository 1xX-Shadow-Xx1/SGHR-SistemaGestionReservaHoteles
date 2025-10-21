using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    [Table("Auditoria")]
    public class Auditory : BaseEntity
    {
        [Column("id_usuario")]
        public int IdUsuario { get; set; }
        [Column("tabla_afectada")]
        public string TablaAfectada { get; set; }
        [Column("operacion")]
        public string Operacion { get; set; } 
        [Column("fecha")]
        public DateTime Fecha { get; set; } = DateTime.Now;
        [Column("detalle")]
        public string Detalle { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public Usuario Usuario { get; set; }

    }
}
