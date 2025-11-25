using SGHR.Web.Data.Interfaces.Reservas;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Reservas;

namespace SGHR.Web.Services.SeviceMonitor.Reservas
{
    public class ReservaMemoryCheck : IReservaMemoryCheck
    {
        private readonly IReservaRepositoryMemory _memory;
        public ReservaMemoryCheck(IReservaRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Reserva/Get-Reservas");
        }
    }
}
