
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class CreateTarifaDto
    {
        public string NombreCategoria { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public decimal Precio { get; set; }
    }
}
