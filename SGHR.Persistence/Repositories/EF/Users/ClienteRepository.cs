using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Users;
using System.Linq.Expressions;


namespace SGHR.Persistence.Repositories.EF.Users
{
    public sealed class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ClienteRepository> _logger;

        public ClienteRepository(SGHRContext context,
                                 ILogger<ClienteRepository> logger,
                                 ILogger<BaseRepository<Cliente>> loggercliente) : base(context,loggercliente)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<OperationResult<Cliente>> SaveAsync(Cliente entity, int? sesionId = null)
        {
            try
            {
                var result = await base.SaveAsync(entity, sesionId);
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

        public override async Task<OperationResult<Cliente>> UpdateAsync(Cliente entity, int? sesionId = null)
        {
            try
            {
                var result = await base.UpdateAsync(entity, sesionId);
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

        public override async Task<OperationResult<Cliente>> DeleteAsync(Cliente entity, int? sesionId = null)
        {
            try
            {
                var result = await base.DeleteAsync(entity, sesionId);
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

        public override async Task<OperationResult<List<Cliente>>> GetAllByAsync(
            Expression<Func<Cliente, bool>> filter, bool includeDeleted = false)
        {
            try
            {
                var result = await base.GetAllByAsync(filter, includeDeleted);
                if (result.Success)
                    _logger.LogInformation("Clientes filtrados obtenidos correctamente, total: {Count}", result.Data.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo clientes filtrados");
                return OperationResult<List<Cliente>>.Fail("Ocurrió un error al obtener los clientes filtrados.");
            }
        }

        public override async Task<OperationResult<bool>> ExistsAsync(
            Expression<Func<Cliente, bool>> filter, bool includeDeleted = false)
        {
            try
            {
                var result = await base.ExistsAsync(filter, includeDeleted);
                _logger.LogInformation("Comprobación de existencia de cliente realizada, resultado: {Exists}", result.Data);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comprobando existencia de cliente");
                return OperationResult<bool>.Fail("Ocurrió un error al comprobar la existencia del cliente.");
            }
        }

        public async Task<OperationResult<Cliente>> GetByCedulaAsync(string cedula)
        {
            try
            {
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Cedula == cedula && !c.Eliminado);

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
                    .Where(c => c.Nombre.Contains(nombre) && !c.Eliminado)
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
