using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Auditory
{
    public class CreateAuditoryDto
    {
        public int IdUsuario { get; set; }
        public string Operacion { get; set; }
        public string Detalle { get; set; }
    }
}
