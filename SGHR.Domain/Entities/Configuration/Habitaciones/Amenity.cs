using SGHR.Domain.Base;


namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    public sealed class Amenity : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Categoria> Categorias { get; set; }
    }
}
