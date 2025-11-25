using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Operaciones;

namespace SGHR.Web.Services.ServiceAPI.Operaciones
{
    public class PagoServiceAPI : IPagoServiceAPI
    {
        private readonly IPagoRepositoryMemory _memory;
        private readonly IClientAPI<PagoModel> _clientAPI;

        public PagoServiceAPI(IPagoRepositoryMemory memory, IClientAPI<PagoModel> clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public async Task<ServicesResultModel> AnularPago(int idPago)
        {
            return await _clientAPI.DeleteAsync($"Pago/Anular-Pago?idPago={idPago}");
        }

        public ServicesResultModel getPagoById(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<PagoModel> getPagoList()
        {
            return _memory.GetModels();
        }

        public async Task<ServicesResultModel> GetResumenDePagos()
        {
            return await _clientAPI.GetResumenPagoAsync("Pago/Get-Resumen-Pagos");
        }

        public async Task<ServicesResultModel> RealizarPago(RealizarPagoModel realizarPago)
        {
            return await _clientAPI.PostAsync("Pago/Realizar-Pago", realizarPago);
        }
    }
}
