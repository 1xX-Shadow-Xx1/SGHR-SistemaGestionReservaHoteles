using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;

namespace SGHR.Web.Services.SeviceMonitor.Reservas
{
    public class ServicioAdicionalMemoryCheck : IServicioAdicionalMemoryCheck
    {
        private readonly IServicioAdicionalRepositoryMemory _memory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServicioAdicionalMemoryCheck(IServicioAdicionalRepositoryMemory memory, IHttpContextAccessor httpContextAccessor)
        {
            _memory = memory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("ServicioAdicional/Get-Servicio-Adicional");
        }
    }
}
