
using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models.EnumsModel.Operaciones
{
    public enum MetodoPagoModel
    {
        Efectivo = 1,
        [Display(Name = "Tarjeta de Credito")]
        TarjetaCredito = 2,
        [Display(Name = "Tarjeta de Debito")]
        TarjetaDebito = 3,
        [Display(Name = "Transferencia Bancaria")]
        TransferenciaBancaria = 4
    }
}
