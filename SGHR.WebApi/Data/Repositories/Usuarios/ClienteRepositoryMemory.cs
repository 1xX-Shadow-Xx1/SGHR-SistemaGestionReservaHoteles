using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Usuarios
{
    public class ClienteRepositoryMemory : BaseRepositoryMemory<ClienteModel>, IClienteRepositoryMemory
    {
        public ClienteRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI) { }

        public override List<ClienteModel> GetModels()
        {
            return base.GetModels();
        }

        public override ApiResult<ClienteModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<ClienteModel>.Ok(result.Data, "Cliente obtenido correctamente.");
            else
                return ApiResult<ClienteModel>.Fail(result.StatusCode, "No se encontro un cliente con ese id");
        }

        public ApiResult<ClienteModel> GetByCedulaModel(string cedula)
        {
            var cliente = baseModelsData.OfType<ClienteModel>().FirstOrDefault(c => c.Cedula == cedula);
            if (cliente == null)
                return ApiResult<ClienteModel>.Fail(400, "No se encontro un cliente con esa cedula");
            else
                return ApiResult<ClienteModel>.Ok(cliente, "Cliente obtenido correctamente.");
        }

        public override async Task<ApiResult<List<ClienteModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
            {
                return ApiResult<List<ClienteModel>>.Ok(result.Data, "Lista de clientes actualizada correctamente.");
            }
            else
            {
                return ApiResult<List<ClienteModel>>.Fail(result.StatusCode, result.Message);
            }

        }
    }
}
