using SGHR.Domain.Entities.Configuration.Reservas;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    public sealed class Habitacion : Base.BaseEntity
    {
        public int IdCategoria { get; set; }
        public int IdPiso { get; set; }
        public string Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; } 

        public Categoria Categoria { get; set; }
        public Piso Piso { get; set; }
        public ICollection<Reserva> Reservas { get; set; }
    }
}
