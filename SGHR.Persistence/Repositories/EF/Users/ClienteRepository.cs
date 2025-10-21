using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Users;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _logger;
        private readonly IConfiguration _configuration;

        public ClienteRepository(SGHRContext context,
                                 ILogger<ClienteRepository> logger,
                                 IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public override async Task<OperationResult<Cliente>> Save(Cliente entity)
        {
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Cliente>> Update(Cliente entity)
        {
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Update(entity);
        }

        public override async Task<OperationResult<Cliente>> Delete(Cliente entity)
        {
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Delete(entity);
        }

        public override async Task<OperationResult<Cliente>> GetById(int id)
        {
            try
            {
                var entity = await _context.Clientes
                .FirstOrDefaultAsync(c => c.ID == id && !c.is_deleted);

                if (entity == null)
                    return OperationResult<Cliente>.Fail("Cliente no encontrado");

                return OperationResult<Cliente>.Ok(entity);
            }
            catch(Exception ex)
            {
                _logger.LogWarning("No se encontró el Cliente con Id {Id}", id);
                return OperationResult<Cliente>.Fail($"Error: {ex.Message}");
            }                            
        }
        public override async Task<OperationResult<List<Cliente>>> GetAll()
        {
            var result = await base.GetAll();
            if (result.Success)
                _logger.LogInformation("Clientes obtenidos correctamente. Total: {Count}", result.Data.Count);
            else
                _logger.LogError("Error al obtener los Clientes: {Message}", result.Message);
            return result;
        }
        public async Task<OperationResult<Cliente>> GetByCedulaAsync(string cedula)
        {
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula && !c.is_deleted);

                if (cliente == null)
                    return OperationResult<Cliente>.Fail("Cliente no encontrado");

                return OperationResult<Cliente>.Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente por cedula");
                return OperationResult<Cliente>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
