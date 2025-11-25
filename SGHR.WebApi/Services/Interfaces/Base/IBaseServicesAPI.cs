using SGHR.Web.Models;

namespace SGHR.Web.Services.Interfaces.Base
{
    public interface IBaseServicesAPI<TModel, TSaveModel, TUpdateModel> where TModel : class where TSaveModel : class where TUpdateModel : class
    {
        List<TModel> GetServices();
        ServicesResultModel GetByIDServices(int id);
        Task<ServicesResultModel> SaveServicesPost(TSaveModel model);
        Task<ServicesResultModel> UpdateServicesPut(TUpdateModel model);
        Task<ServicesResultModel> RemoveServicesPut(int id);
    }
}
