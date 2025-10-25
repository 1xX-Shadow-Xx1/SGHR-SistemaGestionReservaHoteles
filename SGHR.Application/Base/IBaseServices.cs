using SGHR.Application.Dtos.Configuration.Users.Usuario;

namespace SGHR.Application.Base
{
    public interface IBaseServices<Tdtocreate, Tdtoupdate>
    {
        Task<ServiceResult> CreateAsync(Tdtocreate CreateDto);
        Task<ServiceResult> UpdateAsync(Tdtoupdate UpdateDto);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> GetAllAsync();
    }
}
