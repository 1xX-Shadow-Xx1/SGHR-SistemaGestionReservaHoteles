using Microsoft.EntityFrameworkCore;
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

        public HabitacionRepository(SGHRContext context, ILogger<HabitacionRepository> logger)
            : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<List<Habitacion>>> GetDisponiblesAsync()
        {
            try
            {
                var habitaciones = await _context.Habitaciones.Where(h => h.Estado == "Disponible" && !h.IsDeleted).ToListAsync();

                return OperationResult<List<Habitacion>>.Ok(habitaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo habitaciones disponibles");
                return OperationResult<List<Habitacion>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
