using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class AuditoryRepository : BaseRepository<Auditory>, IAuditoryRepository
    {
        public AuditoryRepository(SGHRContext context) : base(context)
        {    
        }

        public async Task<OperationResult<List<Auditory>>> GetByTable(string tableName)
        {
            var result = await GetAll(a => a.TablaAfectada == tableName);
            return result;
        }

        public async Task<OperationResult<List<Auditory>>> GetByUserId(int userId)
        {   
            var result = await GetAll(a => a.IdUsuario == userId);
            return result;
        }
    }
}
