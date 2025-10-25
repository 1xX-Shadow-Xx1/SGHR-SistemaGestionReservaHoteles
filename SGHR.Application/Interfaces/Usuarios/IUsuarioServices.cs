using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;

namespace SGHR.Application.Interfaces.Usuarios
{
    public interface IUsuarioServices : IBaseServices<CreateUsuarioDto, UpdateUsuarioDto>
    {
    }
}
