using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class HabitacionRepositoryMemory : BaseRepositoryMemory<HabitacionModel> , IHabitacionRepositoryMemory
    {
        public HabitacionRepositoryMemory(IClientAPI clienteAPI) : base(clienteAPI)
        {
        }

        public override ApiResult<HabitacionModel> GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ApiResult<HabitacionModel>.Ok(result.Data, "Habitacion obtenida correctamente.");
            else
                return ApiResult<HabitacionModel>.Fail(result.StatusCode, "No se encontro una habitacion con ese id");
        }

        public override List<HabitacionModel> GetModels()
        {
            return base.GetModels();
        }

        public ApiResult<HabitacionModel> GetHabitacionByNumero(string numeroHabitacion)
        {
            var result = baseModelsData.OfType<HabitacionModel>().FirstOrDefault(h => h.Numero ==  numeroHabitacion);
            if (result != null)
                return ApiResult<HabitacionModel>.Ok(result, "Habitacion obtenida correctamente.");
            else
                return ApiResult<HabitacionModel>.Fail(400, "No se encontro una habitacion con ese id");
        }

        public override async Task<ApiResult<List<HabitacionModel>>> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ApiResult<List<HabitacionModel>>.Ok(result.Data, "Lista de habitaciones actualizada correctamente.");
            else
                return ApiResult<List<HabitacionModel>>.Fail(result.StatusCode, result.Message);
        }
    }
}
