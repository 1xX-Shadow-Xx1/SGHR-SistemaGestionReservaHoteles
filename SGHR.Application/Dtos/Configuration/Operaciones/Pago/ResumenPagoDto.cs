
namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class ResumenPagoDto
    {
        public decimal TotalRecaudado { get; set; }
        public decimal TotalRechazado { get; set; }
        public decimal Pendientes { get; set; }
        public decimal Completados { get; set; }
        public decimal Parciales { get; set; }
        public decimal Rechazados { get; set; }
        public decimal TotalPagos { get; set; }
    }
}
