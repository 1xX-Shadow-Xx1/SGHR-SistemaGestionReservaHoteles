using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ReservaRepository : BaseRepository<Reserva>, IReservaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ReservaRepository> _logger;

        public ReservaRepository(SGHRContext context, ILogger<ReservaRepository> logger)
            : base(context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OperationResult<List<Reserva>>> GetReservasActivasAsync()
        {
            try
            {
                var reservas = await _context.Reservas.Where(r => r.Estado == "Activa" && !r.IsDeleted).ToListAsync(); 

                return OperationResult<List<Reserva>>.Ok(reservas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reservas activas");
                return OperationResult<List<Reserva>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
