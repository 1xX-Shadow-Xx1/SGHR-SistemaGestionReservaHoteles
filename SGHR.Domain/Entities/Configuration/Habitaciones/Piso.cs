namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    public sealed class Piso : Base.BaseEntity
    {
        public int NumeroPiso { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; } = "Habilitado";
        public ICollection<Habitacion> Habitaciones { get; set; }

    }
}
