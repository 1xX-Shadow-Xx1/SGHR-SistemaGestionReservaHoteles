using SGHR.Application.Dtos.Configuration.Users.Usuario;

namespace SGHR.Application.Base
{
    public interface IBaseServices<Tdtocreate, Tdtoupdate>
    {
        Task<ServiceResult> CreateAsync(Tdtocreate CreateDto, int? sesionId = null);
        Task<ServiceResult> UpdateAsync(Tdtoupdate UpdateDto, int? sesionId = null);
        Task<ServiceResult> DeleteAsync(int id, int? sesionId = null);
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> GetAllAsync();
    }
}
