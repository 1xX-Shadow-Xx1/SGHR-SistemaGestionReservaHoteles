using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class CategoriaRepositoryMemory : BaseRepositoryMemory<CategoriaModel> , ICategoriaRepositoryMemory
    {
        public CategoriaRepositoryMemory(IClientAPI<CategoriaModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Categoria obtenida correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro una categoria con ese id");
        }

        public override List<CategoriaModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de categorias actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
