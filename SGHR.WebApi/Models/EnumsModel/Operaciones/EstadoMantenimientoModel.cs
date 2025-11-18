using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models.EnumsModel.Operaciones
{
    public enum EstadoMantenimientoModel
    {
        Pendiente = 1,
        [Display(Name = "En proceso")]
        EnProceso = 2,          
        Completado = 3,         
        Cancelado = 4          
    }
}
