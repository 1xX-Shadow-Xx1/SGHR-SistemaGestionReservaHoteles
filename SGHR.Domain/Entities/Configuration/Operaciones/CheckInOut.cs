using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    public class CheckInOut : BaseEntity
    {
        public int IdReserva { get; set; }
        public DateTime? FechaCheckIn { get; set; }
        public DateTime? FechaCheckOut { get; set; }
        public int AtendidoPor { get; set; }

        public Reserva Reserva { get; set; }
        public Usuario Usuario { get; set; }
    }
}
