using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Domain.Entities.Configuration.Usuers;


namespace SGHR.Application.Interfaces.Users
{
    public interface IUsuarioService : IBaseServices<CreateUsuarioDto, UpdateUsuarioDto>
    {
        Task<ServiceResult> LoginAsync(string email, string password);
        Task<ServiceResult> GetUsuarioByIdAsync(int id);
    }
}
