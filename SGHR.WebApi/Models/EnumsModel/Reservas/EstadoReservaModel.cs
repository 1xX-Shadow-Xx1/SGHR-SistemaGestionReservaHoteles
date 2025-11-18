using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models.EnumsModel.Reserva
{
    public enum EstadoReservaModel
    {
        Pendiente = 1,          
        Confirmada = 2,         
        Activa = 3,             
        Finalizada = 4,         
        Cancelada = 5,
        [Display(Name = "Pago parcial")]
        PagoParcial = 6
    }
}
