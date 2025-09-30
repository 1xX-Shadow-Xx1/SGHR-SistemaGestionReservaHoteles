using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Users
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _logger;

        public ClienteRepository(SGHRContext context, ILogger<ClienteRepository> logger)
            : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<Cliente>> GetByCedulaAsync(string cedula)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula && !c.IsDeleted);

                if (cliente == null)
                    return OperationResult<Cliente>.Fail("Cliente no encontrado");

                return OperationResult<Cliente>.Ok(cliente);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente por cedula");
                return OperationResult<Cliente>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
