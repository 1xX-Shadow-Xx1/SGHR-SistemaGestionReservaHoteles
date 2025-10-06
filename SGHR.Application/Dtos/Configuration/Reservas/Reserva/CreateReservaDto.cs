using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Reservas.Reserva
{
    public class CreateReservaDto
    {
        public int IdHabitacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal CostoTotal { get; set; }
    }
}
