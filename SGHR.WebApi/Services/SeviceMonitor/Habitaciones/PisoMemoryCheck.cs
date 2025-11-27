using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Services.SeviceMonitor.Habitaciones
{
    public class PisoMemoryCheck : IPisoMemoryCheck
    {
        private readonly IPisoRepositoryMemory _memory;

        public PisoMemoryCheck(IPisoRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ApiResult<List<PisoModel>>> CheckData()
        {
            return await _memory.CheckDataAPI("Piso/Get-Pisos");
        }
    }
}
