using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Repository;
using SGHR.Domain.Validators.Habitaciones;
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
            var result = HabitacionValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Habitacion>> Update(Habitacion entity)
        {
            var result = HabitacionValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Update(entity);
        }

        public override async Task<OperationResult<Habitacion>> Delete(Habitacion entity)
        {
            var result = HabitacionValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Delete(entity);
        }

        public override async Task<OperationResult<Habitacion>> GetById(int id)
        {
            try
            {
                var entity = await _context.Habitaciones
                    .FirstOrDefaultAsync(h => h.ID == id && !h.is_deleted);

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
                    .Where(h => h.IdCategoria == categoriaId && !h.is_deleted)
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
                    .Where(h => h.Estado == "Disponible" && !h.is_deleted)
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
    }
}
