using SGHR.Domain.Entities.Configuration.Usuers;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Reportes
{
    [Table("Reportes")]
    public sealed class Reporte : Base.BaseEntity
    {
        [Column("tipo_reporte")]
        public string TipoReporte { get; set; }
        [Column("fecha_generacion")]
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        [Column("generado_por")]
        public int GeneradoPor { get; set; }
        [Column("ruta_archivo")]
        public string RutaArchivo { get; set; }

    }
}
