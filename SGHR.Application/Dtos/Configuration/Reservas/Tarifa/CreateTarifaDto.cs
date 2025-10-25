
namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class CreateTarifaDto
    {
        public string NombreCategoria { get; set; }
        public string Temporada { get; set; }
        public decimal Precio { get; set; }
    }
}
