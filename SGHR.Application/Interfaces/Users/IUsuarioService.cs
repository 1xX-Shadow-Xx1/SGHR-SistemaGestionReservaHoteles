using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;


namespace SGHR.Application.Interfaces.Users
{
    public interface IUsuarioService : IBaseServices<CreateUsuarioDto, UpdateUsuarioDto>
    {
    }
}
