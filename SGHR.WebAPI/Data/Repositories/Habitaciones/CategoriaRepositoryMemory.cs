using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class CategoriaRepositoryMemory : BaseRepositoryMemory<CategoriaModel> , ICategoriaRepositoryMemory
    {
        public CategoriaRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<CategoriaModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<CategoriaModel>.Ok(result.Data, "Categoria obtenida correctamente.");
            else
                return ApiResult<CategoriaModel>.Fail(result.StatusCode, "No se encontro una categoria con ese id");
        }

        public override List<CategoriaModel> GetModels()
        {
            return base.GetModels();
        }

        public override async Task<ApiResult<List<CategoriaModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<CategoriaModel>>.Ok(result.Data, "Lista de categorias actualizada correctamente.");
            else
                return ApiResult<List<CategoriaModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
