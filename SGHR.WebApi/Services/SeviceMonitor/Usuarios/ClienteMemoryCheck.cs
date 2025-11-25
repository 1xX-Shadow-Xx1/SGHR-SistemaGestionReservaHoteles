using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;

namespace SGHR.Web.Services.SeviceMonitor.Usuarios
{
    public class ClienteMemoryCheck : IClienteMemoryCheck
    {
        private readonly IClienteRepositoryMemory _memory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteMemoryCheck(IClienteRepositoryMemory memory, IHttpContextAccessor httpContextAccessor)
        {
            _memory = memory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Cliente/Get-Clientes");

        }
    }
}
