using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;

namespace SGHR.Web.Services.SeviceMonitor.Operaciones
{
    public class PagoMemoryCheck : IPagoMemoryCheck
    {
        private readonly IPagoRepositoryMemory _memory;

        public PagoMemoryCheck(IPagoRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ApiResult<List<PagoModel>>> CheckData()
        {
            return await _memory.CheckDataAPI("Pago/Get-Pagos");
        }
    }
}
