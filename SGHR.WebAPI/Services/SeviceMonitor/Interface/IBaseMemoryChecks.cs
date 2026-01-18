
using SGHR.Web.Models;

namespace SGHR.Web.Services.SeviceMonitor.Interface
{
    public interface IBaseMemoryChecks<TModel>
    {
        Task<ApiResult<List<TModel>>> CheckData();
    }
}
