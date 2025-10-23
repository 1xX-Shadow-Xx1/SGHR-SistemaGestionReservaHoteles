using SGHR.Domain.Base;
using System.ComponentModel.DataAnnotations.Schema;


namespace SGHR.Domain.Entities.Configuration.Operaciones
{
    [Table("Mantenimientos")]
    public sealed class Mantenimiento : BaseEntity
    {
        [Column("id_piso")]
        public int? IdPiso { get; set; }
        [Column("id_habitacion")]
        public int IdHabitacion { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
        [Column("fecha_inicio")]
        public DateTime FechaInicio { get; set; }
        [Column("fecha_fin")]
        public DateTime? FechaFin { get; set; }
        [Column("realizado_por")]
        public int RealizadoPor { get; set; }
        [Column("estado")]
        public string Estado { get; set; } = "Iniciado";

    }
}
