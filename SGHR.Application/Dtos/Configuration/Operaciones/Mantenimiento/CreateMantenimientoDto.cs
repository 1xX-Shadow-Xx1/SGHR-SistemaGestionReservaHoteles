using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento
{
    public class CreateMantenimientoDto
    {
        public int? IdPiso { get; set; }
        public int IdHabitacion { get; set; }
        public string Descripcion { get; set; }
        public int RealizadoPor { get; set; }
        public DateTime FechaInicio { get; set; }
    }
}
