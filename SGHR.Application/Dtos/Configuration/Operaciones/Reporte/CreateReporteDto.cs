
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public class CreateReporteDto
    {
        [Required(ErrorMessage = "El tipo de reporte es obligatorio.")]
        [RegularExpression("^(Financiero|Ocupación|Clientes|Reservas|General)$",
        ErrorMessage = "El tipo de reporte debe ser 'Financiero', 'Ocupación', 'Clientes', 'Reservas' o 'General'.")]
        [StringLength(50, ErrorMessage = "El tipo de reporte no puede superar los 50 caracteres.")]
        public string TipoReporte { get; set; }

        [Required(ErrorMessage = "El correo del generador es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string GeneradoPor { get; set; }

        [Required(ErrorMessage = "La ruta del archivo es obligatoria.")]
        [StringLength(255, ErrorMessage = "La ruta del archivo no puede superar los 255 caracteres.")]
        public string RutaArchivo { get; set; }
    }
}
