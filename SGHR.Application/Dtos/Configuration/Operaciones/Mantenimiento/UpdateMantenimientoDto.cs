using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento
{
    public record UpdateMantenimientoDto
    {
        public int Id { get; set; }
        public int? IdPiso { get; set; }
        public int IdHabitacion { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int RealizadoPor { get; set; }
        public string Estado { get; set; }
    }
}
