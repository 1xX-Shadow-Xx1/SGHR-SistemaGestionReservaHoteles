using SGHR.Domain.Entities.Configuration.Reservas;

namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    public sealed class Pago : Base.BaseEntity
    {
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;

        public Reserva Reserva { get; set; }
    }
}
