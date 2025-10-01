using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class HabitacionRepository : BaseRepository<Habitacion>, IHabitacionRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<HabitacionRepository> _logger;
        private readonly IConfiguration _configuration;

        public HabitacionRepository(SGHRContext context,
                                    ILogger<HabitacionRepository> logger,
                                    IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Habitacion>> Save(Habitacion entity)
        {
            var result = await base.Save(entity);
            if (result.Success)
                _logger.LogInformation("Habitación registrada: {Id} - Número {Numero}", entity.Id, entity.Numero);
            else
                _logger.LogError("Error al registrar Habitación: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Habitacion>> Update(Habitacion entity)
        {
            var result = await base.Update(entity);
            if (result.Success)
                _logger.LogInformation("Habitación actualizada: {Id}", entity.Id);
            else
                _logger.LogError("Error al actualizar Habitación {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Habitacion>> Delete(Habitacion entity)
        {
            var result = await base.Delete(entity);
            if (result.Success)
                _logger.LogInformation("Habitación eliminada correctamente: {Id}", entity.Id);
            else
                _logger.LogError("Error al eliminar Habitación {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Habitacion>> GetById(int id)
        {
            try
            {
                var entity = await _context.Habitaciones
                    .Include(h => h.Categoria)
                    .Include(h => h.Piso)
                    .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted);

                if (entity == null)
                    return OperationResult<Habitacion>.Fail("Habitación no encontrada");

                return OperationResult<Habitacion>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Habitación por Id {Id}", id);
                return OperationResult<Habitacion>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Habitacion>>> GetByCategoriaAsync(int categoriaId)
        {
            try
            {
                var lista = await _context.Habitaciones
                    .Include(h => h.Categoria)
                    .Where(h => h.IdCategoria == categoriaId && !h.IsDeleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Habitacion>>.Fail("No se encontraron habitaciones para esta categoría");

                return OperationResult<List<Habitacion>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habitaciones por categoría {CategoriaId}", categoriaId);
                return OperationResult<List<Habitacion>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Habitacion>>> GetDisponiblesAsync()
        {
            try
            {
                var lista = await _context.Habitaciones
                    .Include(h => h.Categoria)
                    .Where(h => h.Estado == "Disponible" && !h.IsDeleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Habitacion>>.Fail("No hay habitaciones disponibles");

                return OperationResult<List<Habitacion>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habitaciones disponibles");
                return OperationResult<List<Habitacion>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Habitacion>>> GetByPisoAsync(int pisoId)
        {
            try
            {
                var lista = await _context.Habitaciones
                    .Include(h => h.Piso)
                    .Where(h => h.IdPiso == pisoId && !h.IsDeleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Habitacion>>.Fail("No se encontraron habitaciones para este piso");

                return OperationResult<List<Habitacion>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener habitaciones por piso {PisoId}", pisoId);
                return OperationResult<List<Habitacion>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
