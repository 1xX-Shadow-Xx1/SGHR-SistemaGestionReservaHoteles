

using SGHR.Web.Models;

namespace SGHR.Web.Services.ClienteAPIService.Interface
{
    public interface IClientAPI<T> where T : class
    {
        Task<ServicesResultModel> GetListAsync(string endpoint);
        Task<ServicesResultModel> PostAsync(string endpoint, object? data = null);
        Task<ServicesResultModel> PutAsync(string endpoint, object? data = null);
        Task<ServicesResultModel> DeleteAsync(string endpoint);
        Task<ServicesResultModel> GetAsync(string endpoint);
        Task<ServicesResultModel> GetSesionAsync(string endpoint);
        Task<ServicesResultModel> GetResumenPagoAsync(string endpoint);
    }

}