namespace SGHR.Domain.Entities.Configuration.Reservas
{
    public sealed class ServicioAdicional : Base.BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}
