using SGHR.Web.Models.EnumsModel.Operaciones;

namespace SGHR.Web.Models.Operaciones.Pago
{
    public class RealizarPagoModel
    {
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public MetodoPagoModel MetodoPago { get; set; }
    }
}
