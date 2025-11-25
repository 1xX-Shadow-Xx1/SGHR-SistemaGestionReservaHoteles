using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;

namespace SGHR.Web.Services.SeviceMonitor.Reservas
{
    public class TarifaMemoryCheck : ITarifaMemoryCheck
    {
        private readonly ITarifaRepositoryMemory _memory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TarifaMemoryCheck(ITarifaRepositoryMemory memory, IHttpContextAccessor httpContextAccessor)
        {
            _memory = memory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Tarifa/Get-Tarifas");
        }
    }
}
