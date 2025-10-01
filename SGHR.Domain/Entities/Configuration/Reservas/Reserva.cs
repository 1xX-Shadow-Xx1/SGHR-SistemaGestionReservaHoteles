using SGHR.Domain.Entities.Configuration;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Entities.Configuration.Usuers;

namespace SGHR.Domain.Entities.Configuration.Reservas
{
    public sealed class Reserva : Base.BaseEntity
    {
        public int IdCliente { get; set; }
        public int IdHabitacion { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal CostoTotal { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public Habitacion Habitacion { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Pago> Pagos { get; set; }
    }
}
