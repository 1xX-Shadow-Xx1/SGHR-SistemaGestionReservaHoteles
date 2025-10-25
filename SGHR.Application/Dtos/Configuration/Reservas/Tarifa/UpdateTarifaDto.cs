

namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public record UpdateTarifaDto
    {
        public int Id { get; set; }
        public string? NombreCategoria { get; set; }
        public string? Temporada { get; set; }
        public decimal? Precio { get; set; }
    }
}
