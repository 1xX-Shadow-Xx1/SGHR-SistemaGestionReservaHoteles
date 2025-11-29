using SGHR.Web.Data.Interfaces.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;

namespace SGHR.Web.Data.Interfaces.Usuarios
{
    public interface IClienteRepositoryMemory : IBaseRepositoryMemory<ClienteModel>
    {
        ApiResult<ClienteModel> GetByCedulaModel(string cedula);
    }
}
