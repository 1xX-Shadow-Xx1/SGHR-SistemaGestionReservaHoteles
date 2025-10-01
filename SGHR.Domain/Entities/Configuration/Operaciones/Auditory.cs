using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    public class Auditory : BaseEntity
    {
        public int IdUsuario { get; set; }
        public string TablaAfectada { get; set; }
        public string Operacion { get; set; } 
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Detalle { get; set; }

        public Usuario Usuario { get; set; }
    }
}
