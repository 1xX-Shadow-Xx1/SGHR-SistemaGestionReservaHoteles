using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using System.Linq.Expressions;


namespace SGHR.Application.Interfaces.Users
{
    public interface IUsuarioService : IBaseServices<UsuarioCreateDto, UsuarioUpdateDto>
    {
        Task<ServiceResult> GetAllByAsync(Expression<Func<UsuarioDto, bool>> filter);
        Task<ServiceResult> ExistsAsync(Expression<Func<UsuarioDto, bool>> filter);
        Task<ServiceResult> GetByCorreoAsync(string correo);
        Task<ServiceResult> GetByRolAsync(string rol);
        Task<ServiceResult> GetActivosAsync();
        Task<ServiceResult> LoginAsync(UsuarioLoginDto usuarioLoginDto);
    }
}
