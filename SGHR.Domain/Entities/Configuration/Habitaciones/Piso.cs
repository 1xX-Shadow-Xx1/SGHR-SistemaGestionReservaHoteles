using SGHR.Domain.Enum.Habitaciones;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    [Table("Pisos")]
    public sealed class Piso : Base.BaseEntity
    {
        [Column("numero_piso")]
        public int NumeroPiso { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("estado")]
        public EstadoPiso Estado { get; set; } = EstadoPiso.Habilitado;

    }
}
