using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;


namespace SGHR.Application.Interfaces.Users
{
    public interface IUsuarioService : IBaseServices<UsuarioCreateDto, UsuarioUpdateDto>
    {
        Task<ServiceResult> GetByCorreoAsync(string correo);
        Task<ServiceResult> GetByRolAsync(string rol);
        Task<ServiceResult> GetActivosAsync();
        Task<ServiceResult> LoginAsync(UsuarioLoginDto usuarioLoginDto);
        Task<ServiceResult> GetAllByAsync(string? nombre = null, string? rol = null, string? estado = null);
    }
}
