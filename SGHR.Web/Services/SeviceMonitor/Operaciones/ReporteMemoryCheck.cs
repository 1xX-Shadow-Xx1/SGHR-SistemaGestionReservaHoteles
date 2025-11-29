using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;

namespace SGHR.Web.Services.SeviceMonitor.Operaciones
{
    public class ReporteMemoryCheck : IReporteMemoryCheck
    {
        private readonly IReporteRepositoryMemory _memory;

        public ReporteMemoryCheck(IReporteRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ApiResult<List<ReporteModel>>> CheckData()
        {
            return await _memory.CheckDataAPI("Reporte/get-Reportes");
        }
    }
}
