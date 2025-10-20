using SGHR.Domain.Entities.Configuration.Reservas;
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
        [Column("numero")]
        public string Numero { get; set; }
        [Column("capacidad")]
        public int Capacidad { get; set; }
        [Column("estado")]
        public string Estado { get; set; } 

        public Categoria Categoria { get; set; }
        public Piso Piso { get; set; }
        public ICollection<Reserva> Reservas { get; set; }

    }
}
