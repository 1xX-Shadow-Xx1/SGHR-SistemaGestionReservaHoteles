using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Habitaciones
{
    public interface ICategoriaServiceAPI : IBaseServicesAPI<CategoriaModel, CreateCategoriaModel, UpdateCategoriaModel>
    {
    }
}
