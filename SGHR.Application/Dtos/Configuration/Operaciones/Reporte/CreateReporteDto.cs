using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public class CreateReporteDto
    {
        public string TipoReporte { get; set; }
        public int GeneradoPor { get; set; }
        public string RutaArchivo { get; set; }
    }
}
