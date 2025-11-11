using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.ConfigurationRules.Habitaciones;
using SGHR.Persistence.Context;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class HabitacionRepository : BaseRepository<Habitacion>, IHabitacionRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<HabitacionRepository> _logger;
        private readonly HabitacionValidator _habitacionValidator;

        public HabitacionRepository(SGHRContext context,
                                    HabitacionValidator habitacionValidator,
                                    ILogger<HabitacionRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
            _habitacionValidator = habitacionValidator;

        }

        public override async Task<OperationResult<Habitacion>> SaveAsync(Habitacion entity)
        {
            if (!_habitacionValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar la Habitacion: {fail}", errorMessage);
                return OperationResult<Habitacion>.Fail(errorMessage);
            }
            _logger.LogInformation("Guardando nueva habitación...");
            var result = await base.SaveAsync(entity);

            if (result.Success)
                _logger.LogInformation("Habitación guardada correctamente con Id {Id}", entity.Id);
            else
                _logger.LogWarning("Error al guardar habitación: {Message}", result.Message);

            return result;
        }
        public override async Task<OperationResult<Habitacion>> UpdateAsync(Habitacion entity)
        {
            if (!_habitacionValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar la Habitacion: {fail}", errorMessage);
                return OperationResult<Habitacion>.Fail(errorMessage);
            }

            _logger.LogInformation("Actualizando habitación con Id {Id}", entity.Id);
            var result = await base.UpdateAsync(entity);

            if (result.Success)
                _logger.LogInformation("Habitación actualizada correctamente con Id {Id}", entity.Id);
            else
                _logger.LogWarning("Error al actualizar habitación: {Message}", result.Message);

            return result;
        }
        public override async Task<OperationResult<Habitacion>> DeleteAsync(Habitacion entity)
        {
            _logger.LogInformation("Eliminando habitación con Id {Id}", entity.Id);
            var result = await base.DeleteAsync(entity);

            if (result.Success)
                _logger.LogInformation("Habitación eliminada correctamente con Id {Id}", entity.Id);
            else
                _logger.LogWarning("Error al eliminar habitación: {Message}", result.Message);

            return result;
        }
        public override async Task<OperationResult<Habitacion>> GetByIdAsync(int id, bool includeDeleted = false)
        {
            _logger.LogInformation("Obteniendo habitación por Id {Id}", id);
            return await base.GetByIdAsync(id, includeDeleted);
        }
        public override async Task<OperationResult<List<Habitacion>>> GetAllAsync(bool includeDeleted = false)
        {

            var habitaciones = await base.GetAllAsync(includeDeleted);
            if (!habitaciones.Success)
            {
                _logger.LogInformation("Habitaciones obtenidas correctamente.");
                return habitaciones;
            }

            _logger.LogInformation("Habitaciones obtenidas correctamente.");
            return habitaciones;
        }
        public async Task<OperationResult<List<Habitacion>>> GetAvailableAsync(DateTime fechaInicio, DateTime fechaFin, int? idreserva = 0)
        {
            try
            {
                _logger.LogInformation("Obteniendo habitaciones disponibles entre {Inicio} y {Fin}", fechaInicio, fechaFin);

                var disponibles = await _context.Habitaciones
                .Where(h => !h.IsDeleted &&
                            !_context.Reservas.Any(r =>
                                !r.IsDeleted &&
                                (
                                    r.Estado == Domain.Enum.Reservas.EstadoReserva.Confirmada ||
                                    r.Estado == Domain.Enum.Reservas.EstadoReserva.PagoParcial ||
                                    r.Estado == Domain.Enum.Reservas.EstadoReserva.Activa
                                ) &&
                                r.IdHabitacion == h.Id &&
                                (
                                    (fechaInicio >= r.FechaInicio && fechaInicio <= r.FechaFin) ||
                                    (fechaFin >= r.FechaInicio && fechaFin <= r.FechaFin) ||
                                    (fechaInicio <= r.FechaInicio && fechaFin >= r.FechaFin)
                                ) &&
                                r.Id != idreserva))
                .ToListAsync();


                _logger.LogInformation("Se encontraron {Cantidad} habitaciones disponibles", disponibles.Count);

                return OperationResult<List<Habitacion>>.Ok(disponibles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo habitaciones disponibles");
                return OperationResult<List<Habitacion>>.Fail("Ocurrió un error al obtener las habitaciones disponibles");
            }
        }
        public async Task<OperationResult<List<Habitacion>>> GetByCategoriaAsync(int idCategoria, bool includeDeleted = false)
        {
            try
            {
                _logger.LogInformation("Obteniendo habitaciones por categoría {CategoriaId}", idCategoria);

                var habitaciones = await _context.Habitaciones
                    .Where(h => h.IdCategoria == idCategoria && (includeDeleted || !h.IsDeleted))
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Cantidad} habitaciones en la categoría {CategoriaId}", habitaciones.Count, idCategoria);

                return OperationResult<List<Habitacion>>.Ok(habitaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo habitaciones por categoría {CategoriaId}", idCategoria);
                return OperationResult<List<Habitacion>>.Fail("Ocurrió un error al obtener las habitaciones por categoría");
            }
        }
        public async Task<OperationResult<List<Habitacion>>> GetByPisoAsync(int idPiso, bool includeDeleted = false)
        {
            try
            {
                _logger.LogInformation("Obteniendo habitaciones del piso {PisoId}", idPiso);

                var habitaciones = await _context.Habitaciones
                    .Where(h => h.IdPiso == idPiso && (includeDeleted || !h.IsDeleted))
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Cantidad} habitaciones en el piso {PisoId}", habitaciones.Count, idPiso);

                return OperationResult<List<Habitacion>>.Ok(habitaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo habitaciones del piso {PisoId}", idPiso);
                return OperationResult<List<Habitacion>>.Fail("Ocurrió un error al obtener las habitaciones del piso");
            }
        }
    }
}
