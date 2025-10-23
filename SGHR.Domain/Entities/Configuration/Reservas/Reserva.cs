using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Reservas
{
    [Table("Reservas")]
    public sealed class Reserva : Base.BaseEntity
    {
        [Column("id_cliente")]
        public int IdCliente { get; set; }
        [Column("id_habitacion")]
        public int IdHabitacion { get; set; }
        [Column("id_usuario")]  
        public int IdUsuario { get; set; }
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("fecha_fin")]
        public DateTime FechaFin { get; set; }
        [Column("costo_total")]
        public decimal CostoTotal { get; set; }
        [Column("estado")]
        public string Estado { get; set; }

    }
}
