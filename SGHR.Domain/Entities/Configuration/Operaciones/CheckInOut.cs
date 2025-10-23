using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


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
