using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Operaciones;

namespace SGHR.Web.Services.SeviceMonitor.Operaciones
{
    public class MantenimientoMemoryCheck : IMantenimientoMemoryCheck
    {
        private readonly IMantenimientoRepositoryMemory _memory;

        public MantenimientoMemoryCheck(IMantenimientoRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Mantenimiento/Get-All");
        }
    }
}
