using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Habitaciones;
using System.Collections.Generic;

namespace SGHR.Web.Services.ServiceAPI.Habitaciones
{
    public class HabitacionServiceAPI : IHabitacionServiceAPI
    {
        private readonly IHabitacionRepositoryMemory _memory;
        private readonly IClientAPI _clientAPI;

        public HabitacionServiceAPI(IHabitacionRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ApiResult<HabitacionModel> GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<HabitacionModel> GetServices()
        {
            return _memory.GetModels();
        }

        public ApiResult<HabitacionModel> GetHabitacionByNumero(string numeroHabitacion)
        {
            return _memory.GetHabitacionByNumero(numeroHabitacion);
        }

        public async Task<ApiResult<HabitacionModel>> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync<HabitacionModel>($"Habitacion/Remove-Habitacion?id={id}");
        }

        public async Task<ApiResult<HabitacionModel>> SaveServicesPost(CreateHabitacionModel model)
        {
            return await _clientAPI.PostAsJsonAsync<CreateHabitacionModel, HabitacionModel>("Habitacion/Create-Habitacion", model);
        }

        public async Task<ApiResult<HabitacionModel>> UpdateServicesPut(UpdateHabitacionModel model)
        {
            return await _clientAPI.PutAsJsonAsync<UpdateHabitacionModel, HabitacionModel>("Habitacion/Update-Habitacion", model);
        }

        public async Task<ApiResult<HabitacionModel>> GetHabitacionesDisponibles()
        {
            return await _clientAPI.GetAsync<HabitacionModel>("Habitacion/Get-Habitaciones-disponibles");
        }

        public async Task<ApiResult<List<HabitacionModel>>> GetHabitacionesDisponiblesRangeDate(DateTime startDate, DateTime endDate)
        {
            return await _clientAPI.GetAsync<List<HabitacionModel>>($"Habitacion/Get-Habitaciones-disponibles-date?fechainicio={startDate}&fechafin={endDate}");
        }
    }
}
