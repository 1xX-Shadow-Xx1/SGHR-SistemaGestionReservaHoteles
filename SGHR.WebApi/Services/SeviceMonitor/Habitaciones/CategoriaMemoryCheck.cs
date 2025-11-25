using SGHR.Web.Data.Interfaces.Habitaciones;
using SGHR.Web.Models;
using SGHR.Web.Services.SeviceMonitor.Interface.Habitaciones;

namespace SGHR.Web.Services.SeviceMonitor.Habitaciones
{
    public class CategoriaMemoryCheck : ICategoriaMemoryCheck
    {
        private readonly ICategoriaRepositoryMemory _memory;

        public CategoriaMemoryCheck(ICategoriaRepositoryMemory memory)
        {
            _memory = memory;
        }

        public async Task<ServicesResultModel> CheckData()
        {
            return await _memory.CheckDataAPI("Categoria/Get-Categorias");
        }
    }
}
