using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;

namespace SGHR.Application.Interfaces.Usuarios
{
    public interface IClienteServices : IBaseServices<CreateClienteDto,UpdateClienteDto>
    {
        Task<ServiceResult> GetByNameAsync(string name);
    }
}
