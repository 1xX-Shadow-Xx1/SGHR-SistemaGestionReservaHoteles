using SGHR.Application.Base;

namespace SGHR.Application.Interfaces
{
    public interface IDashboardServices
    {
        Task<ServiceResult> GetDashboardDataAsync();
    }
}