using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;

namespace SGHR.Application.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<ServiceResult> LoginSesionAsync(string correo, string password);
        Task<ServiceResult> CloseSesionAsync(int idusuario);
        Task<ServiceResult> RegistrarUsuarioAsync(CreateUsuarioDto createUsuarioDto);
    }
}
