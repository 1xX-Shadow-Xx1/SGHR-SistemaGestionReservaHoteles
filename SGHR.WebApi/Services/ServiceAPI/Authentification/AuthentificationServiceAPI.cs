using SGHR.Web.Models;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Authentification;

namespace SGHR.Web.Services.ServiceAPI.Authentification
{
    public class AuthentificationServiceAPI : IAuthentificationServiceAPI
    {
        private readonly IClientAPI<SesionLoginModel> _clientAPI;
        private readonly IHttpContextAccessor _httpContex;

        public AuthentificationServiceAPI(IClientAPI<SesionLoginModel> clientAPI, IHttpContextAccessor httpContex)
        {
            _clientAPI = clientAPI;
            _httpContex = httpContex;
        }

        public async Task<ServicesResultModel> CheckSesionAsync()
        {
            return await _clientAPI.GetSesionAsync($"Sesion/CheckSesionActivityByUserID?userId={_httpContex.HttpContext.Session.GetInt32("UserId")}");
        }

        public async Task<ServicesResultModel> CloseSesionAsync()
        {
            return await _clientAPI.PutAsync($"Authentication/Authentication-CloseSesion?id={_httpContex.HttpContext.Session.GetInt32("UserId")}");
        }

        public async Task<ServicesResultModel> LoginAsync(string nameUser, string passwordUser)
        {
            return await _clientAPI.PutAsync($"Authentication/Authentication-Login?correo={nameUser}&contraseña={passwordUser}");
        }

        public async Task<ServicesResultModel> RegisterAsync(CreateUsuarioModel model)
        {
            return await _clientAPI.PostAsync("Authentication/Authentication-Register", model);
        }

        public async Task<ServicesResultModel> UpdateActivitySesionAsync()
        {
            return await _clientAPI.PutAsync($"Sesion/UpdateActivitySesionByUser?userId={_httpContex.HttpContext.Session.GetInt32("UserId")}");
        }
    }
}
