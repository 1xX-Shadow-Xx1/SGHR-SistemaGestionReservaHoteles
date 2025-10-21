using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    [Table("CheckInOut")]
    public class CheckInOut : BaseEntity
    {
        [Column("id_reserva")]
        public int IdReserva { get; set; }
        [Column("fecha_checkin")]
        public DateTime? FechaCheckIn { get; set; }
        [Column("fecha_checkout")]
        public DateTime? FechaCheckOut { get; set; }
        [Column("atendido_por")]
        public int AtendidoPor { get; set; }

    }
}
