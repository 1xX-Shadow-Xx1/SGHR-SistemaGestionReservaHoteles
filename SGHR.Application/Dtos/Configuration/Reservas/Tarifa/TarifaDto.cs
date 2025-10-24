namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class TarifaDto
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Temporada { get; set; }
        public decimal Precio { get; set; }
    }
}
