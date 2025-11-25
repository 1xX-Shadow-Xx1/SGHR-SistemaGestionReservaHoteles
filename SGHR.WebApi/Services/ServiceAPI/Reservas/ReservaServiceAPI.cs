using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Reservas;

namespace SGHR.Web.Services.ServiceAPI.Reservas
{
    public class ReservaServiceAPI : IReservaServiceAPI
    {
        private readonly IClientAPI<ReservaModel> _clientAPI;
        private readonly IReservaRepositoryMemory _memory;
        private readonly IServicioAdicionalRepositoryMemory _servicioMemory;

        public ReservaServiceAPI(IClientAPI<ReservaModel> clientAPI, 
                                            IReservaRepositoryMemory memory, 
                                            IServicioAdicionalRepositoryMemory repositoryMemory)
        {
            _clientAPI = clientAPI;
            _memory = memory;
            _servicioMemory = repositoryMemory;
        }

        public ServicesResultModel GetByIDServices(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<ReservaModel> GetServices()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> RemoveServicesPut(int id)
        {
            return await _clientAPI.DeleteAsync($"Reserva/Remove-Reserva?id={id}");
        }

        public async Task<ServicesResultModel> SaveServicesPost(CreateReservaModel model)
        {
            return await _clientAPI.PostAsync("Reserva/Create-Reserva", model);
        }

        public async Task<ServicesResultModel> UpdateServicesPut(UpdateReservaModel model)
        {
            return await _clientAPI.PutAsync("Reserva/Update-Reserva", model);
        }

        public async Task<ServicesResultModel> AddServicio_ReservaPut(string nameServicio, int idreserva)
        {
            return await _clientAPI.PutAsync($"Reserva/Add-Servicio-Adicional-to-Reserva?id={idreserva}&nameServicio={nameServicio}");
        }

        public async Task<ServicesResultModel> RemoveServicio_ReservaPut(string nameServicio, int idreserva)
        {
            return await _clientAPI.PutAsync($"Reserva/Remove-Servicio-Adicional-to-Reserva?id={idreserva}&nombreServicio={nameServicio}");
        }

        public async Task<ServicesResultModel> GetServicesbyReserva(int idreserva)
        {
            return await _clientAPI.GetListAsync($"Reserva/Get-Servicios-By-ReservaID?id={idreserva}");
        }

        public List<ServicioAdicionalModel> GetServiciosAdicionalesdisponibles()
        {
            return _servicioMemory.GetModels();
        }
    }
}
