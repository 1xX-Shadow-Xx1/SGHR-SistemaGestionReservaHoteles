using SGHR.Web.Areas.Administrador.Models;
using SGHR.Web.Models;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces;

namespace SGHR.Web.Services.ServiceAPI
{
    public class DashBoardServiceAPI : IDashBoardServiceAPI
    {
        private readonly IClientAPI<DashboardViewModel> _clientAPI;
        public DashBoardServiceAPI(IClientAPI<DashboardViewModel> clientAPI)
        {
            _clientAPI = clientAPI;
        }
        public async Task<ServicesResultModel> GetDashBoard()
        {
            return await _clientAPI.GetAsync("DashBoard/GetDataDashBoard");
        }
    }
}
