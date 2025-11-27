using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Usuarios
{
    public class UsuarioRepositoryMemory : BaseRepositoryMemory<UsuarioModel>,  IUsuarioRepositoryMemory
    {

        public UsuarioRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<UsuarioModel> GetByIDModel(int id)
        {
            return base.GetByIDModel(id);
        }
        public override List<UsuarioModel> GetModels()
        {
            return base.GetModels();
        }
        public override async Task<ApiResult<List<UsuarioModel>>> CheckDataAPI(string endpoint)
        {
            return await base.CheckDataAPI(endpoint);
        }

    }
}
