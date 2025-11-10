using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGHR.Domain.Entities.Configuration.Reservas
{
    [Table("Tarifas")]
    public sealed class Tarifa : BaseEntity
    {
        [Column("id_categoria")]
        public int IdCategoria { get; set; }
        [Column("fecha_inicio")]
        public DateTime Fecha_inicio { get; set; }
        [Column("fecha_fin")]
        public DateTime Fecha_fin { get; set; }
        [Column("precio")]
        public decimal Precio { get; set; }
        

    }
}
