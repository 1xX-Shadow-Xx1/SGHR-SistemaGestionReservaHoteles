using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Usuarios
{
    public class ClienteRepositoryMemory : BaseRepositoryMemory<ClienteModel>, IClienteRepositoryMemory
    {
        public ClienteRepositoryMemory(IClientAPI<ClienteModel> clienteAPI) : base(clienteAPI) { }

        public override List<ClienteModel> GetModels()
        {
            return base.GetModels();
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Cliente obtenido correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro un cliente con ese id");
        }

        public ServicesResultModel GetByCedulaModel(string cedula)
        {
            var cliente = baseModelsData.OfType<ClienteModel>().FirstOrDefault(c => c.Cedula == cedula);
            if (cliente == null)
                return ServicesResultModel.Fail(400, "No se encontro un cliente con esa cedula");
            else
                return ServicesResultModel.Ok(200, cliente, "Cliente obtenido correctamente.");
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
            {
                return ServicesResultModel.Ok(result.Statuscode, "Lista de clientes actualizada correctamente.");
            }
            else
            {
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
            }

        }
    }
}
