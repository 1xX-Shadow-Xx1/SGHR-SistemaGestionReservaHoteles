using SGHR.Domain.Entities.Configuration.Reservas;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    public sealed class Categoria : Base.BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Habitacion> Habitaciones { get; set; }
        public ICollection<Tarifa> Tarifas { get; set; }
        public ICollection<Amenities> Amenities { get; set; }
    }
}
