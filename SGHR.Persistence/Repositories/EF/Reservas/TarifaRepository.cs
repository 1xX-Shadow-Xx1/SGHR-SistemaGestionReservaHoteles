using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.ConfigurationRules.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reservas;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class TarifaRepository : BaseRepository<Tarifa>, ITarifaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<TarifaRepository> _logger;
        private readonly TarifaValidator _tarifaValidator;

        public TarifaRepository(SGHRContext context,
                                TarifaValidator tarifaValidator,
                                ILogger<TarifaRepository> logger,
                                ILogger<BaseRepository<Tarifa>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
            _tarifaValidator = tarifaValidator;
        }

        public override async Task<OperationResult<Tarifa>> SaveAsync(Tarifa entity)
        {
            if (!_tarifaValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al guardar la Tarifa: {fail}", errorMessage);
                return OperationResult<Tarifa>.Fail(errorMessage);
            }
            try
            {
                var result = await base.SaveAsync(entity);
                _logger.LogInformation("Se guardó la tarifa con ID {Id}", result.Data?.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando tarifa");
                return OperationResult<Tarifa>.Fail("Ocurrió un error al guardar la tarifa");
            }
        }
        public override async Task<OperationResult<Tarifa>> UpdateAsync(Tarifa entity)
        {
            if (!_tarifaValidator.Validate(entity, out string errorMessage))
            {
                _logger.LogWarning("Fallo al actualizar la Tarifa: {fail}", errorMessage);
                return OperationResult<Tarifa>.Fail(errorMessage);
            }
            try
            {
                var result = await base.UpdateAsync(entity);
                _logger.LogInformation("Se actualizó la tarifa con ID {Id}", result.Data?.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando tarifa");
                return OperationResult<Tarifa>.Fail("Ocurrió un error al actualizar la tarifa");
            }
        }
        public override async Task<OperationResult<Tarifa>> DeleteAsync(Tarifa entity)
        {
            try
            {
                var result = await base.DeleteAsync(entity);
                _logger.LogInformation("Se eliminó la tarifa con ID {Id}", entity.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando tarifa");
                return OperationResult<Tarifa>.Fail("Ocurrió un error al eliminar la tarifa");
            }
        }
        public async Task<OperationResult<List<Tarifa>>> GetByCategoriaAsync(int idCategoria)
        {
            try
            {
                var tarifas = await _context.Tarifa
                    .Where(t => t.IdCategoria == idCategoria && !t.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} tarifas para la categoría {CategoriaId}", tarifas.Count, idCategoria);
                return OperationResult<List<Tarifa>>.Ok(tarifas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tarifas para la categoría {CategoriaId}", idCategoria);
                return OperationResult<List<Tarifa>>.Fail("Ocurrió un error al obtener las tarifas");
            }
        }
        public async Task<OperationResult<Tarifa>> GetByCategoriaAndTemporadaAsync(int idCategoria, string temporada)
        {
            try
            {
                var tarifa = await _context.Tarifa
                    .FirstOrDefaultAsync(t => t.IdCategoria == idCategoria && t.Temporada == temporada && !t.IsDeleted);

                if (tarifa == null)
                {
                    _logger.LogWarning("No se encontró tarifa para la categoría {CategoriaId} y temporada {Temporada}", idCategoria, temporada);
                    return OperationResult<Tarifa>.Fail("Tarifa no encontrada");
                }

                _logger.LogInformation("Se obtuvo tarifa para la categoría {CategoriaId} y temporada {Temporada}", idCategoria, temporada);
                return OperationResult<Tarifa>.Ok(tarifa);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tarifa para la categoría {CategoriaId} y temporada {Temporada}", idCategoria, temporada);
                return OperationResult<Tarifa>.Fail("Ocurrió un error al obtener la tarifa");
            }
        }
        public async Task<OperationResult<List<Tarifa>>> GetByTemporadaAsync(string temporada)
        {
            try
            {
                var tarifas = await _context.Tarifa
                    .Where(t => t.Temporada == temporada && !t.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Se obtuvieron {Count} tarifas para la temporada {Temporada}", tarifas.Count, temporada);
                return OperationResult<List<Tarifa>>.Ok(tarifas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tarifas para la temporada {Temporada}", temporada);
                return OperationResult<List<Tarifa>>.Fail("Ocurrió un error al obtener las tarifas");
            }
        }

    }
}
