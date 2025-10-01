using SGHR.Domain.Entities.Configuration.Usuers;

namespace SGHR.Domain.Entities.Configuration.Reportes
{
    public sealed class Reporte : Base.BaseEntity
    {
        public string TipoReporte { get; set; }
        public DateTime FechaGeneracion { get; set; } = DateTime.Now;
        public int GeneradoPor { get; set; }
        public string RutaArchivo { get; set; }

        public Usuario Usuario { get; set; }

    }
}
