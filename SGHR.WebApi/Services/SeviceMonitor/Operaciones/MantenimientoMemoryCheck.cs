using SGHR.Web.Data.Interfaces.Operaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Models.Usuarios.Usuario;
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

        public async Task<ApiResult<List<MantenimientoModel>>> CheckData()
        {
            return await _memory.CheckDataAPI("Mantenimiento/Get-All");
        }
    }
}
