using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Habitaciones;

namespace SGHR.Web.Services.ServiceAPI.Habitaciones
{
    public class HabitacionServiceAPI : IHabitacionServiceAPI
    {
        private readonly IHabitacionRepositoryMemory _memory;
        private readonly IClientAPI<HabitacionModel> _clientAPI;

        public HabitacionServiceAPI(IHabitacionRepositoryMemory memory, IClientAPI<HabitacionModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<HabitacionModel> GetServices()
        {
            return _memory.GetModels();
        }

        public ServicesResultModel GetHabitacionByNumero(string numeroHabitacion)
        {
            return _memory.GetHabitacionByNumero(numeroHabitacion);
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Habitacion/Remove-Habitacion?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateHabitacionModel model)
        {
            return await _clientAPI.PostAsync("Habitacion/Create-Habitacion", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateHabitacionModel model)
        {
            return await _clientAPI.PutAsync("Habitacion/Update-Habitacion", model);
        }

        public async Task<ServicesResultModel> GetHabitacionesDisponibles()
        {
            return await _clientAPI.GetListAsync("Habitacion/Get-Habitaciones-disponibles");
        }

        public async Task<ServicesResultModel> GetHabitacionesDisponiblesRangeDate(DateTime startDate, DateTime endDate)
        {
            return await _clientAPI.GetListAsync($"Habitacion/Get-Habitaciones-disponibles-date?fechainicio={startDate}&fechafin={endDate}");
        }
    }
}
