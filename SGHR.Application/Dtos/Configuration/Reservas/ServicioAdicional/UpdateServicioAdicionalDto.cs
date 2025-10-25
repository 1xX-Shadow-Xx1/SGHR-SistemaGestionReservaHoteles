namespace SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional
{
    public record UpdateServicioAdicionalDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
    }
}
