using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models.EnumsModel.Operaciones
{
    public enum EstadoMantenimientoModel
    {
        Pendiente,
        [Display(Name = "En proceso")]
        EnProceso,          
        Completado,         
        Cancelado          
    }
}
