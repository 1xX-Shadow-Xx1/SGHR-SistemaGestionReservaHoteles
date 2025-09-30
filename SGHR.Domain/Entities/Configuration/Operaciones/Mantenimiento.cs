using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    public sealed class Mantenimiento : BaseEntity
    {
        public int? IdPiso { get; set; }
        public int IdHabitacion { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int RealizadoPor { get; set; }

        public Piso Piso { get; set; }
        public Habitacion Habitacion { get; set; }
        public Usuario Usuario { get; set; }
    }
}
