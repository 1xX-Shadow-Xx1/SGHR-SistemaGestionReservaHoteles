using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Data.Repositories.Base;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.ClienteAPIService.Interface;

namespace SGHR.Web.Data.Repositories.Habitaciones
{
    public class HabitacionRepositoryMemory : BaseRepositoryMemory<HabitacionModel> , IHabitacionRepositoryMemory
    {
        public HabitacionRepositoryMemory(IClientAPI<HabitacionModel> clienteAPI) : base(clienteAPI)
        {
        }

        public override ServicesResultModel GetByIDModel(int id)
        {
            var result = base.GetByIDModel(id);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, result.Data, "Habitacion obtenida correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, "No se encontro una habitacion con ese id");
        }

        public override List<HabitacionModel> GetModels()
        {
            return base.GetModels();
        }

        public ServicesResultModel GetHabitacionByNumero(string numeroHabitacion)
        {
            var result = baseModelsData.OfType<HabitacionModel>().FirstOrDefault(h => h.Numero ==  numeroHabitacion);
            if (result != null)
                return ServicesResultModel.Ok(200, result, "Habitacion obtenida correctamente.");
            else
                return ServicesResultModel.Fail(400, "No se encontro una habitacion con ese id");
        }

        public override async Task<ServicesResultModel> CheckDataAPI(string endpoint)
        {
            var result = await base.CheckDataAPI(endpoint);
            if (result.Success)
                return ServicesResultModel.Ok(result.Statuscode, "Lista de habitaciones actualizada correctamente.");
            else
                return ServicesResultModel.Fail(result.Statuscode, result.Message);
        }
    }
}
