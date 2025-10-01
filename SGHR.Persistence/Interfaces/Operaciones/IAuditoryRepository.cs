using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Operaciones
{
    public interface IAuditoryRepository : IBaseRepository<Auditory>
    {
        Task<OperationResult<List<Auditory>>> GetByUserId(int userId);
        Task<OperationResult<List<Auditory>>> GetByTable(string tableName);
    }
}
