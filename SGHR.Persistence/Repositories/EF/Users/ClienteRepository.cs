using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.ConfigurationRules.Users;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces.Users;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _logger;
        private readonly ClienteValidator _clienteValidator;

        public ClienteRepository(SGHRContext context,
                                 ClienteValidator clienteValidator,
                                 ILogger<ClienteRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
            _clienteValidator = clienteValidator;
        }

        public override async Task<OperationResult<Cliente>> SaveAsync(Cliente entity)
        {
            if (!_clienteValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar Cliente: {fail}", errorMessage);
                return OperationResult<Cliente>.Fail(errorMessage);
            }

            try
            {
                var result = await base.SaveAsync(entity);
                if (result.Success)
                    _logger.LogInformation("Cliente {Nombre} creado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando el cliente {Nombre}", entity.Nombre);
                return OperationResult<Cliente>.Fail("Ocurrió un error al guardar el cliente.");
            }
        }
        public override async Task<OperationResult<Cliente>> UpdateAsync(Cliente entity)
        {
            if (!_clienteValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar el Cliente: {fail}", errorMessage);
                return OperationResult<Cliente>.Fail(errorMessage);
            }
            try
            {
                var result = await base.UpdateAsync(entity);
                if (result.Success)
                    _logger.LogInformation("Cliente {Nombre} actualizado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el cliente {Nombre}", entity.Nombre);
                return OperationResult<Cliente>.Fail("Ocurrió un error al actualizar el cliente.");
            }
        }
        public override async Task<OperationResult<Cliente>> DeleteAsync(Cliente entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);
                if (result.Success)
                    _logger.LogInformation("Cliente {Nombre} eliminado correctamente.", entity.Nombre);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando el cliente {Nombre}", entity.Nombre);
                return OperationResult<Cliente>.Fail("Ocurrió un error al eliminar el cliente.");
            }
        }
        public override async Task<OperationResult<Cliente>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetByIdAsync(id, includeDeleted);
                if (result.Success)
                    _logger.LogInformation("Cliente con ID {Id} obtenido correctamente.", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo el cliente con ID {Id}", id);
                return OperationResult<Cliente>.Fail("Ocurrió un error al obtener el cliente.");
            }
        }
        public override async Task<OperationResult<List<Cliente>>> GetAllAsync(bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetAllAsync(includeDeleted);
                if (result.Success)
                    _logger.LogInformation("Clientes obtenidos correctamente, total: {Count}", result.Data.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo todos los clientes");
                return OperationResult<List<Cliente>>.Fail("Ocurrió un error al obtener los clientes.");
            }
        }
        public async Task<OperationResult<Cliente>> GetByCedulaAsync(string cedula)
        {
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Cedula == cedula && !c.IsDeleted);

                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con cédula {Cedula} no encontrado", cedula);
                    return OperationResult<Cliente>.Fail("Cliente no encontrado");
                }

                _logger.LogInformation("Cliente con cédula {Cedula} obtenido correctamente", cedula);
                return OperationResult<Cliente>.Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo cliente por cédula {Cedula}", cedula);
                return OperationResult<Cliente>.Fail("Ocurrió un error al obtener el cliente");
            }
        }
        public async Task<OperationResult<List<Cliente>>> GetByNombreAsync(string nombre)
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.Nombre.Contains(nombre) && !c.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Clientes filtrados por nombre '{Nombre}' obtenidos correctamente, total: {Count}", nombre, clientes.Count);
                return OperationResult<List<Cliente>>.Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo clientes por nombre {Nombre}", nombre);
                return OperationResult<List<Cliente>>.Fail("Ocurrió un error al obtener los clientes por nombre");
            }
        }

    }
}
