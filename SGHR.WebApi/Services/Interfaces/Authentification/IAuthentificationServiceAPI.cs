using SGHR.Web.Models;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;

namespace SGHR.Web.Services.Interfaces.Authentification
{
    public interface IAuthentificationServiceAPI
    {
        Task<ApiResult<SesionLoginModel>> LoginAsync(string nameUser, string passwordUser);
        Task<ApiResult<SesionLoginModel>> RegisterAsync(CreateUsuarioModel model);
        Task<ApiResult<dynamic>> CloseSesionAsync();
        Task<ApiResult<CheckSesionModel>> CheckSesionAsync();
        Task<ApiResult<CheckSesionModel>> UpdateActivitySesionAsync();
    }
}
