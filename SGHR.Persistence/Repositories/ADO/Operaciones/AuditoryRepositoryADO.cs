using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.ADO.Operaciones
{
    public sealed class AuditoryRepositoryADO : BaseRepository<Auditory>, IAuditoryRepository
    {
        public AuditoryRepositoryADO(SGHRContext context) : base(context)
        {
        }

        public Task<OperationResult<List<Auditory>>> GetByTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult<List<Auditory>>> GetByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
