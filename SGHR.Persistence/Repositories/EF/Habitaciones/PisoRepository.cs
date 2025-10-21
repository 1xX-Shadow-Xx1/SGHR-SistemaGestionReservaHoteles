using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.Habitaciones;
using SGHR.Domain.Validators.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class PisoRepository : BaseRepository<Piso>, IPisoRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<PisoRepository> _logger;
        private readonly IConfiguration _configuration;

        public PisoRepository(SGHRContext context,
                              ILogger<PisoRepository> logger,
                              IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Piso>> Save(Piso entity)
        {
            var result = PisoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Piso>> Update(Piso entity)
        {
            var result = PisoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Update(entity);
        }

        public override async Task<OperationResult<Piso>> Delete(Piso entity)
        {
            var result = PisoValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Delete(entity);
        }

        public override async Task<OperationResult<Piso>> GetById(int id)
        {
            try
            {
                var entity = await _context.Piso
                    .FirstOrDefaultAsync(p => p.ID == id && !p.is_deleted);

                if (entity == null)
                    return OperationResult<Piso>.Fail("Piso no encontrado");

                return OperationResult<Piso>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Piso por Id {Id}", id);
                return OperationResult<Piso>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Piso>>> GetPisosHabilitadosAsync()
        {
            try
            {
                var lista = await _context.Piso
                    .Where(p => p.Estado == "Habilitado" && !p.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Piso>>.Fail("No hay pisos habilitados");

                return OperationResult<List<Piso>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pisos habilitados");
                return OperationResult<List<Piso>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<Piso>> GetByNumeroAsync(int numero)
        {
            try
            {
                var entity = await _context.Piso
                    .FirstOrDefaultAsync(p => p.NumeroPiso == numero && !p.is_deleted);

                if (entity == null)
                    return OperationResult<Piso>.Fail("Piso no encontrado con ese número");

                return OperationResult<Piso>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Piso por número {Numero}", numero);
                return OperationResult<Piso>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
