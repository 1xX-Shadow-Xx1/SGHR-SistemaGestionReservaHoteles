using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Repository
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<OperationResult<Usuario>> GetByCorreoAsync(string correo);
    }
}
