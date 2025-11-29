using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;

namespace SGHR.Web.Services.Interfaces.Base
{
    public interface IBaseServicesAPI<TModel, TSaveModel, TUpdateModel> where TModel : class where TSaveModel : class where TUpdateModel : class
    {
        List<TModel> GetServices();
        ApiResult<TModel> GetByIDServices(int id);
        Task<ApiResult<TModel>> SaveServicesPost(TSaveModel model);
        Task<ApiResult<TModel>> UpdateServicesPut(TUpdateModel model);
        Task<ApiResult<TModel>> RemoveServicesPut(int id);
    }
}
