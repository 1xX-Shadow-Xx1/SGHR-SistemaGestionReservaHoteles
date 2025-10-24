using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;

namespace SGHR.Application.Interfaces.Users
{
    public interface IClienteService : IBaseServices<CreateClienteDto, UpdateClienteDto>
    {
        Task<ServiceResult> GetByCedulaAsync(string cedula);
    }
}
