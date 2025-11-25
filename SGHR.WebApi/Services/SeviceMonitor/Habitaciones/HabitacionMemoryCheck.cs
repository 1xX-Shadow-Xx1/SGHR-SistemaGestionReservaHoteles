using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Services.SeviceMonitor.Habitaciones
{
    public class HabitacionMemoryCheck : IHabitacionMemoryCheck
    {
        private readonly IHabitacionRepositoryMemory _memory;

        public HabitacionMemoryCheck(IHabitacionRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Habitacion/Get-Habitaciones");
        }
    }
}
