
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Models.Reservas.Reserva;
using System.Text.Json.Serialization;

namespace SGHR.Web.Areas.Administrador.Models
{
    public class DashboardViewModel
    {
        [JsonPropertyName("resumenPago")]
        public ResumenPagoModel ResumenPago { get; set; } = new();
        [JsonPropertyName("pago")]
        public List<PagoModel> Pago { get; set; }
        [JsonPropertyName("reserva")]
        public List<ReservaModel> Reserva { get; set; }
    }
}
