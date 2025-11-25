using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Usuarios
{
    public interface IUsuarioServiceAPI : IBaseServicesAPI<UsuarioModel, CreateUsuarioModel , UpdateUsuarioModel>
    {
    }
}
