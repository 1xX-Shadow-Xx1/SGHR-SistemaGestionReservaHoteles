using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    [Table("Amenities")]
    public sealed class Amenity : BaseEntity
    {
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("precio")]
        public decimal Precio { get; set; } 
        [Column("por_capadidad")]
        public decimal PorCapacidad { get; set; }
    }
}
