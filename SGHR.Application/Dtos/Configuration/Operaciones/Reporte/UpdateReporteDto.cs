using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Reporte
{
    public record UpdateReporteDto
    {
        [Required(ErrorMessage = "El ID del reporte es obligatorio.")]
        public int Id { get; set; }

        [RegularExpression("^(Financiero|Ocupación|Clientes|Reservas|General)$",
            ErrorMessage = "El tipo de reporte debe ser 'Financiero', 'Ocupación', 'Clientes', 'Reservas' o 'General'.")]
        [StringLength(50, ErrorMessage = "El tipo de reporte no puede superar los 50 caracteres.")]
        public string? TipoReporte { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(100, ErrorMessage = "El correo no puede superar los 100 caracteres.")]
        public string? GeneradoPor { get; set; }

        [StringLength(255, ErrorMessage = "La ruta del archivo no puede superar los 255 caracteres.")]
        public string? RutaArchivo { get; set; }
    }
}
