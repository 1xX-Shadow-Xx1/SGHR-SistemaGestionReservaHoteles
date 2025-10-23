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
            _logger.LogInformation("Iniciando con el registro del cliente.");

            _logger.LogInformation("Iniciando con la validacion de datos.");
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                _logger.LogWarning("Problema con la validacion de los datos: {Message}", result.Message);
                return result;
            }
            _logger.LogInformation("Datos validados, iniciando con el registro del cliente.");
            var opResult = await base.Save(entity);
            if (!opResult.Success)
            {
                _logger.LogError("Error al guardar al cliente: {ERROR}", opResult.Message);
                return opResult;
            }
            _logger.LogInformation("Cliente registrado correctamente.");
            return opResult;
        }
        public override async Task<OperationResult<Cliente>> Update(Cliente entity)
        {
            _logger.LogInformation("Iniciando con la actualizacion de los datos de cliente.");
            _logger.LogInformation("Iniciando la validacion de los datos que se actualizaran.");
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                _logger.LogWarning("Problema con la vaalidacion de los datos: {Message}", result.Message);
                return result;
            }
            var opResult = await base.Update(entity);
            if(!opResult.Success)
            {
                _logger.LogError("Error al actualizar los datos del cliente: {ERROR}", opResult.Message);
                return opResult;
            }
            _logger.LogInformation("Cliente actualizado correctamente.");
            return opResult;
        }
        public override async Task<OperationResult<Cliente>> Delete(Cliente entity)
        {
            _logger.LogInformation("Iniciando eliminacion logica del cliente.");
            _logger.LogInformation("Validando datos antes de eliminar.");
            var result = ClienteValidator.Validate(entity);
            if (!result.Success)
            {
                _logger.LogWarning("Problema de validacion del cliente: {message}", result.Message);
                return result;
            }

            _logger.LogInformation("Datos validados, iniciando con la eliminacion logica.");
            var opResult = await base.Delete(entity);
            if (!opResult.Success)
            {
                _logger.LogWarning("Error al elimar al cliente: {ERROR}", opResult.Message);
                return opResult;
            }
            _logger.LogInformation("Cliente eliminado correctamente");
            return opResult;
        }
        public override async Task<OperationResult<Cliente>> GetById(int id)
        {
            _logger.LogInformation("Iniciando la obtencion del cliente con el ID {id}", id);
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
            _logger.LogInformation("Iniciando obtencion de Clientes por cedula.");
            try
            {
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cedula == cedula && !c.is_deleted);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente no encontrado.");
                    return OperationResult<Cliente>.Fail("Cliente no encontrado"); 
                }

                _logger.LogInformation("Cliente encontrado con correctamente.");
                return OperationResult<Cliente>.Ok(cliente,"Se obtuvo al cliente correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente por cedula");
                return OperationResult<Cliente>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
