using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Authentification;

namespace SGHR.Web.Services.ServiceAPI.Authentification
{
    public class AuthentificationServiceAPI : IAuthentificationServiceAPI
    {
        private readonly IClientAPI _clientAPI;
        private readonly IHttpContextAccessor _httpContex;

        public AuthentificationServiceAPI(IClientAPI clientAPI, IHttpContextAccessor httpContex)
        {
            _clientAPI = clientAPI;
            _httpContex = httpContex;
        }

        public async Task<ApiResult<CheckSesionModel>> CheckSesionAsync()
        {
            return await _clientAPI.GetAsync<CheckSesionModel>($"Sesion/CheckSesionActivityByUserID?userId={_httpContex.HttpContext.Session.GetInt32("UserId")}");
        }

        public async Task<ApiResult<dynamic>> CloseSesionAsync()
        {
            return await _clientAPI.PutAsync<dynamic>($"Authentication/Authentication-CloseSesion?id={_httpContex.HttpContext.Session.GetInt32("UserId")}", null);
        }

        public async Task<ApiResult<SesionLoginModel>> LoginAsync(string nameUser, string passwordUser)
        {
            return await _clientAPI.PutAsync<SesionLoginModel>($"Authentication/Authentication-Login?correo={nameUser}&contraseña={passwordUser}");
        }

        public async Task<ApiResult<SesionLoginModel>> RegisterAsync(CreateUsuarioModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateUsuarioModel, SesionLoginModel>("Authentication/Authentication-Register", model);
        }

        public async Task<ApiResult<CheckSesionModel>> UpdateActivitySesionAsync()
        {
            return await _clientAPI.PutAsync<CheckSesionModel>($"Sesion/UpdateActivitySesionByUser?userId={_httpContex.HttpContext.Session.GetInt32("UserId")}");
        }
    }
}
