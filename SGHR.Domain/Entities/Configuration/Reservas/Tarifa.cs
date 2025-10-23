using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGHR.Domain.Entities.Configuration.Reservas
{
    [Table("Tarifas")]
    public sealed class Tarifa : BaseEntity
    {
        [Column("id_categoria")]
        public int IdCategoria { get; set; }
        [Column("temporada")]
        public string Temporada { get; set; }
        [Column("precio")]
        public decimal Precio { get; set; }

    }
}
