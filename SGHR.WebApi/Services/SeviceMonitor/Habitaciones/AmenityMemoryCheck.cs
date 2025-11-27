using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Services.SeviceMonitor.Habitaciones
{
    public class AmenityMemoryCheck : IAmenityMemoryCheck
    {
        private readonly IAmenityRepositoryMemory _memory;

        public AmenityMemoryCheck(IAmenityRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ApiResult<List<AmenityModel>>> CheckData()
        {
            return await _memory.CheckDataAPI("Amenity/Get-Amenity");
        }
    }
}
