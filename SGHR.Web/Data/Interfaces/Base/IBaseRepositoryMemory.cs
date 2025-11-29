using SGHR.Web.Models;

namespace SGHR.Web.Data.Interfaces.Base
{
    public interface IBaseRepositoryMemory<TModel> where TModel : class
    {
        List<TModel> GetModels();
        ApiResult<TModel> GetByIDModel(int id);
        Task<ApiResult<List<TModel>>> CheckDataAPI(string endpoint);
    }
}
