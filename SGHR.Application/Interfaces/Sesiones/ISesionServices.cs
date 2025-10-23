using SGHR.Application.Base;

namespace SGHR.Application.Interfaces.Sesiones
{
    public interface ISesionServices 
    {
        Task<ServiceResult> OpenSesion(int id);
        Task<ServiceResult> CloseSesion();
        Task<ServiceResult> GetSesion();
        Task<ServiceResult> GetSesionByUsers(string correo);
        Task<ServiceResult> GetOpenSesion();
    }
}
