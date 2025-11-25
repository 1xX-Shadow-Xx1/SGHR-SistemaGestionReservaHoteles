
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.Interfaces.Base;

namespace SGHR.Web.Services.Interfaces.Usuarios
{
    public interface IClienteServiceAPI : IBaseServicesAPI<ClienteModel, CreateClienteModel, UpdateClienteModel>
    {
        ServicesResultModel GetByCedulaCliente(string cedula);
    }
}
