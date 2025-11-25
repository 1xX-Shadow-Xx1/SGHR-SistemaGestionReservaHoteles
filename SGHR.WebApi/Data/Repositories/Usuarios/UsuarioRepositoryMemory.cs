using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Usuarios
{
    public class UsuarioRepositoryMemory : BaseRepositoryMemory<UsuarioModel>,  IUsuarioRepositoryMemory
    {
        public UsuarioRepositoryMemory(IClientAPI<UsuarioModel> clienteAPI) : base(clienteAPI) { }

        public override ServicesResultModel GetByIDModel(int id)
        {
            return base.GetByIDModel(id);
        }
        public override List<UsuarioModel> GetModels()
        {
            return base.GetModels();
        }
        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if(result != null && result.Success)
            {
                return ServicesResultModel.Ok(result.Statuscode, result.Message);
            }
            else
            {
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
            }
        }
    }
}
