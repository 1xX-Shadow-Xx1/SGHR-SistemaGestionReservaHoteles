using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;

namespace SGHR.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<ServiceResult> LoginSesionAsync(string correo, string password);
        Task<ServiceResult> CloseSerionAsync(int idusuario);
        Task<ServiceResult> RegistrarUsuario(CreateUsuarioDto createUsuarioDto);
    }
}
