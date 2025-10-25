using SGHR.Domain.Entities.Configuration.Reportes;

namespace SGHR.Domain.Validators.ConfigurationRules.Operaciones
{
    public class ReporteValidator
    {
        public bool Validate(Reporte reporte, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(reporte, "Reporte", out errorMessage)) return false;

            // Tipo de reporte
            if (!ValidationHelper.Required(reporte.TipoReporte, "TipoReporte", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(reporte.TipoReporte, 50, "TipoReporte", out errorMessage)) return false;

            // Fecha de generacion
            if (reporte.FechaGeneracion == DateTime.MinValue)
            {
                errorMessage = "Fecha de generación es obligatoria.";
                return false;
            }

            // Generado por (FK Usuario)
            if (!ValidationHelper.GreaterThanZero(reporte.GeneradoPor, "GeneradoPor", out errorMessage)) return false;

            // Ruta de archivo (opcional)
            if (!string.IsNullOrEmpty(reporte.RutaArchivo))
            {
                if (!ValidationHelper.MaxLength(reporte.RutaArchivo, 255, "RutaArchivo", out errorMessage)) return false;

                // Validación opcional de extensiones (comenta/ajusta según tu caso)
                var allowedExt = new[] { ".pdf", ".xlsx", ".csv" };
                if (!allowedExt.Any(ext => reporte.RutaArchivo.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    errorMessage = "La ruta del archivo debe apuntar a un archivo con extensión válida (.pdf, .xlsx, .csv).";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
