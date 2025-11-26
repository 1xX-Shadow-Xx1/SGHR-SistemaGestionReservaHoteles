using SGHR.Web.Models;

namespace SGHR.Web.Services.Interfaces
{
    public interface IDashBoardServiceAPI
    {
        Task<ServicesResultModel> GetDashBoard();
    }
}
