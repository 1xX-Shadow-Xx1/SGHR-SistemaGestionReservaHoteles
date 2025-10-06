using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion
{
    public class CreateHabitacionDto
    {
        public int IdPiso { get; set; }
        public string Numero { get; set; }
        public int Capacidad { get; set; }
    }
}
