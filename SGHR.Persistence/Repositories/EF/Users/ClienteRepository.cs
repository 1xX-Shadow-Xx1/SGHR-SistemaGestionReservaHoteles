using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
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

            if (result.Success)
                _logger.LogInformation("Cliente creado correctamente: {Id} - {Nombre}", entity.Id, entity.Nombre);
            else
                _logger.LogError("Error al crear Cliente {Nombre}: {Message}", entity.Nombre, result.Message);

            return result;
        }

        public override async Task<OperationResult<Cliente>> Update(Cliente entity)
        {
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Cliente actualizado correctamente: {Id} - {Nombre}", entity.Id, entity.Nombre);
            else
                _logger.LogError("Error al actualizar Cliente {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Cliente>> Delete(Cliente entity)
        {
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Cliente eliminado (soft delete): {Id} - {Nombre}", entity.Id, entity.Nombre);
            else
                _logger.LogError("Error al eliminar Cliente {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Cliente>> GetById(int id)
        {
            var result = await base.GetById(id);

            if (result.Success)
                _logger.LogInformation("Cliente encontrado: {Id}", id);
            else
                _logger.LogWarning("No se encontró el Cliente con Id {Id}", id);

            return result;
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
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula && !c.IsDeleted);

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
