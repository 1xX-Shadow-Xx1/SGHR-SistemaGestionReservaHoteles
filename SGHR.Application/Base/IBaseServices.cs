

using SGHR.Domain.Base;

namespace SGHR.Application.Base
{
    public interface IBaseServices<TDtoCreate , TDtoUpdate, TDtoDelete>
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> Update(TDtoUpdate dto);
        Task<ServiceResult> Remove(TDtoDelete dto);
        Task<ServiceResult> Save(TDtoCreate dto);
    }
}
