using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    [Table("Pagos")]
    public sealed class Pago : Base.BaseEntity
    {
        [Column("id_reserva")]
        public int IdReserva { get; set; }
        [Column("monto")]
        public decimal Monto { get; set; }
        [Column("metodo_pago")]
        public string MetodoPago { get; set; }
        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; } = DateTime.Now;

    }
}
