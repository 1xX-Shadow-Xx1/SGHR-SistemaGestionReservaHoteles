using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.Reservas;
using SGHR.Domain.Validators.Users;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class TarifaRepository : BaseRepository<Tarifa>, ITarifaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<TarifaRepository> _logger;
        private readonly IConfiguration _configuration;

        public TarifaRepository(SGHRContext context,
                                ILogger<TarifaRepository> logger,
                                IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Tarifa>> Save(Tarifa entity)
        {
            var result = TarifaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Tarifa creada: {Id} - {Temporada} - Precio {Precio}", entity.Id, entity.Temporada, entity.Precio);
            else
                _logger.LogError("Error al crear Tarifa: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Tarifa>> Update(Tarifa entity)
        {
            var result = TarifaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Tarifa actualizada: {Id} - {Temporada} - Precio {Precio}", entity.Id, entity.Temporada, entity.Precio);
            else
                _logger.LogError("Error al actualizar Tarifa {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Tarifa>> Delete(Tarifa entity)
        {
            var result = TarifaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Tarifa eliminada (soft delete): {Id} - {Temporada}", entity.Id, entity.Temporada);
            else
                _logger.LogError("Error al eliminar Tarifa {Id}: {Message}", entity.Id, result.Message);

            return result;
        }

        public override async Task<OperationResult<Tarifa>> GetById(int id)
        {
            var result = await base.GetById(id);

            if (result.Success)
                _logger.LogInformation("Tarifa encontrada: {Id}", id);
            else
                _logger.LogWarning("No se encontró Tarifa con Id {Id}", id);

            return result;
        }
        public async Task<OperationResult<List<Tarifa>>> GetByTemporadaAsync(string temporada)
        {
            try
            {
                var tarifas = await _context.Tarifa
                    .Where(t => t.Temporada == temporada && !t.IsDeleted)
                    .ToListAsync();

                if (!tarifas.Any())
                    return OperationResult<List<Tarifa>>.Fail("No se encontraron tarifas para esa temporada");

                return OperationResult<List<Tarifa>>.Ok(tarifas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tarifas por temporada {Temporada}", temporada);
                return OperationResult<List<Tarifa>>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
