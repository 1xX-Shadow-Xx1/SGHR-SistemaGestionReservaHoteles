using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;

namespace SGHR.Web.Services.Interfaces.Authentification
{
    public interface IAuthentificationServiceAPI
    {
        Task<ServicesResultModel> LoginAsync(string nameUser, string passwordUser);
        Task<ServicesResultModel> RegisterAsync(CreateUsuarioModel model);
        Task<ServicesResultModel> CloseSesionAsync();
        Task<ServicesResultModel> CheckSesionAsync();
        Task<ServicesResultModel> UpdateActivitySesionAsync();
    }
}
