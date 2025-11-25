
using SGHR.Web.Models;

namespace SGHR.Web.Services.SeviceMonitor.Interface
{
    public interface IBaseMemoryChecks
    {
        Task<ServicesResultModel> CheckData();
    }
}
