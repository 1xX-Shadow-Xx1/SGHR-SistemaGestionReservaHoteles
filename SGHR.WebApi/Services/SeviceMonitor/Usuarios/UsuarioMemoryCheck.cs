using SGHR.Web.Data.Interfaces.Usuarios;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Usuarios;

namespace SGHR.Web.Services.SeviceTwoPlane.Usuarios
{
    public class UsuarioMemoryCheck : IUsuarioMemoryCheck
    {
        private readonly IUsuarioRepositoryMemory _memory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioMemoryCheck(IUsuarioRepositoryMemory memory, IHttpContextAccessor httpContextAccessor)
        {
            _memory = memory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Usuario/Get-Usuarios");
        }
    }
}
