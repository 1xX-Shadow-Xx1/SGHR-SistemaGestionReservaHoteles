using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class CheckInOutRepository : BaseRepository<CheckInOut>, ICheckInOutRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CheckInOutRepository> _logger;
        private readonly IConfiguration _configuration;

        public CheckInOutRepository(SGHRContext context,
                                    ILogger<CheckInOutRepository> logger,
                                    IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<CheckInOut>> Save(CheckInOut entity)
        {
            return await base.Save(entity);
        }

        public override async Task<OperationResult<CheckInOut>> Update(CheckInOut entity)
        {
            return await base.Update(entity);
        }

        public override async Task<OperationResult<CheckInOut>> Delete(CheckInOut entity)
        {
            return await base.Delete(entity);
        }

        public override async Task<OperationResult<CheckInOut>> GetById(int id)
        {
            try
            {
                var entity = await _context.CheckInOut
                    .FirstOrDefaultAsync(c => c.ID == id && !c.is_deleted);

                if (entity == null)
                    return OperationResult<CheckInOut>.Fail("CheckInOut no encontrado");

                _logger.LogInformation("CheckInOut encontrado: {Id}", id);
                return OperationResult<CheckInOut>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckInOut por Id {Id}", id);
                return OperationResult<CheckInOut>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<CheckInOut>>> GetCheckInsActivosAsync()
        {
            try
            {
                var lista = await _context.CheckInOut
                    .Where(c => c.FechaCheckOut == null && !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<CheckInOut>>.Fail("No hay CheckIns activos");

                return OperationResult<List<CheckInOut>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckIns activos");
                return OperationResult<List<CheckInOut>>.Fail($"Error: {ex.Message}");
            }
        }

        // Obtener CheckOuts en un rango de fechas
        public async Task<OperationResult<List<CheckInOut>>> GetCheckOutsByFechaAsync(DateTime inicio, DateTime fin)
        {
            try
            {
                var lista = await _context.CheckInOut
                    .Where(c => c.FechaCheckOut >= inicio && c.FechaCheckOut <= fin && !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<CheckInOut>>.Fail("No se encontraron CheckOuts en este rango de fechas");

                return OperationResult<List<CheckInOut>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener CheckOuts entre {Inicio} y {Fin}", inicio, fin);
                return OperationResult<List<CheckInOut>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
