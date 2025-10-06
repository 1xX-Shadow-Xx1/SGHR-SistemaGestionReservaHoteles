using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public record UpdateReporteDto
    {
        public int Id { get; set; }
        public string TipoReporte { get; set; }
        public DateTime FechaGeneracion { get; set; } 
        public int GeneradoPor { get; set; }
        public string RutaArchivo { get; set; }
    }
}
