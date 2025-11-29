using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Operaciones
{
    public interface IMantenimientoServiceAPI : IBaseServicesAPI<MantenimientoModel, CreateMantenimientoModel, UpdateMantenimientoModel>
    {
    }
}
