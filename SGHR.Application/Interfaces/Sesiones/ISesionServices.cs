using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;

namespace SGHR.Application.Interfaces.Sesiones
{
    public interface ISesionServices 
    {
        Task<ServiceResult> OpenSesionAsync(StartSesionDto startSesionDto);
        Task<ServiceResult> CloseSesionAsync(CloseSesionDto closeSesionDto, int? idsesion = null);
        Task<ServiceResult> GetSesionAsync();
        Task<ServiceResult> GetSesionByUsersAsync(string correo);
        Task<ServiceResult> GetOpenSesionAsync();
        Task<ServiceResult> DeleteAsync(int id, int? sesionId = null);
        Task<ServiceResult> GetByIdAsync(int id);
    }
}
