using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    [Table("Habitaciones")]
    public sealed class Habitacion : Base.BaseEntity
    {
        [Column("id_categoria")]
        public int IdCategoria { get; set; }
        [Column("id_piso")]
        public int IdPiso { get; set; }
        [Column("id_amenity")]
        public int? IdAmenity { get; set; }
        [Column("numero")]
        public string Numero { get; set; }
        [Column("capacidad")]
        public int Capacidad { get; set; }
        [Column("estado")]
        public string Estado { get; set; } = "Activa";

    }
}
