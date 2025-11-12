
using SGHR.Application.Base;

namespace SGHR.Application.Interfaces.Sesion
{
    public interface ISesionServices
    {
        Task<ServiceResult> GetSesionByIdUser(int idUser);
        Task<ServiceResult> CloseSesionAsync(int idUsuario);
        Task<ServiceResult> CheckActivitySesionByUserAsync(int idUsuario);
        Task<ServiceResult> CheckActivitySesionGlobalAsync();
        Task<ServiceResult> UpdateActivitySesionByUserAsync(int idsesion);
    }
}
