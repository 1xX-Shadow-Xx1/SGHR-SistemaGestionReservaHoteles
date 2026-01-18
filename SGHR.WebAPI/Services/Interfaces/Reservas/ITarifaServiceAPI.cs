using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Reservas
{
    public interface ITarifaServiceAPI : IBaseServicesAPI<TarifaModel, CreateTarifaModel, UpdateTarifaModel>
    {
    }
}
