using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Base
{
    public interface IBaseServices<TDtoCreate, TDtoUpdate, TDtoDelete>
    {
        Task<ServiceResult> GetAll();
        Task<ServiceResult> GetById(int id);
        Task<ServiceResult> Update(TDtoUpdate dto);
        Task<ServiceResult> Remove(TDtoDelete dto);
        Task<ServiceResult> Save(TDtoCreate dto);
    }
}
