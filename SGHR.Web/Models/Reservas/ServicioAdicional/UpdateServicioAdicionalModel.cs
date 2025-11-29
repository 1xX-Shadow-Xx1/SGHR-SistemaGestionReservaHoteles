using SGHR.Web.Models.EnumsModel.Reserva;

namespace SGHR.Web.Models.Reservas.ServicioAdicional
{
    public record UpdateServicioAdicionalModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public EstadoServicioAdicionalModel Estado { get; set; }
    }
}
