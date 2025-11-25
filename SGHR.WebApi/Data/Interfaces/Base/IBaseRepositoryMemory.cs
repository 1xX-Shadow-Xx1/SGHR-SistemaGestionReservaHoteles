using SGHR.Web.Models;

namespace SGHR.Web.Data.Interfaces.Base
{
    public interface IBaseRepositoryMemory<TModel> where TModel : class
    {
        List<TModel> GetModels();
        ServicesResultModel GetByIDModel(int id);
        Task<ServicesResultModel> CheckDataAPI(string endpoint);
    }
}
