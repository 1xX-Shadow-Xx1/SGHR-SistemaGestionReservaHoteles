using SGHR.Web.Areas.Administrador.Models;
using SGHR.Web.Models;

namespace SGHR.Web.Services.Interfaces
{
    public interface IDashBoardServiceAPI
    {
        Task<ApiResult<DashboardViewModel>> GetDashBoard();
    }
}
