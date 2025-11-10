namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class TarifaDto
    {
        public int Id { get; set; }
        public string NombreCategoria { get; set; }
        public DateTime Fecha_inicio { get; set; }
        public DateTime Fecha_fin { get; set; }
        public decimal Precio { get; set; }
    }
}
