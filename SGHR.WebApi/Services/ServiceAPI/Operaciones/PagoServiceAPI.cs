using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Services.ClienteAPIService.Interface;
using SGHR.Web.Services.Interfaces.Operaciones;

namespace SGHR.Web.Services.ServiceAPI.Operaciones
{
    public class PagoServiceAPI : IPagoServiceAPI
    {
        private readonly IPagoRepositoryMemory _memory;
        private readonly IClientAPI _clientAPI;

        public PagoServiceAPI(IPagoRepositoryMemory memory, IClientAPI clientAPI)
        {
            _memory = memory;
            _clientAPI = clientAPI;
        }

        public async Task<ApiResult<PagoModel>> AnularPago(int idPago)
        {
            return await _clientAPI.DeleteAsync<PagoModel>($"Pago/Anular-Pago?idPago={idPago}");
        }

        public ApiResult<PagoModel> getPagoById(int id)
        {
            return _memory.GetByIDModel(id);
        }

        public List<PagoModel> getPagoList()
        {
            return _memory.GetModels();
        }

        public async Task<ApiResult<ResumenPagoModel>> GetResumenDePagos()
        {
            return await _clientAPI.GetAsync<ResumenPagoModel>("Pago/Get-Resumen-Pagos");
        }

        public async Task<ApiResult<PagoModel>> RealizarPago(RealizarPagoModel realizarPago)
        {
            return await _clientAPI.PostAsJsonAsync<RealizarPagoModel, PagoModel>("Pago/Realizar-Pago", realizarPago);
        }
    }
}
