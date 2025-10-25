using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reservas;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class TarifaRepository : BaseRepository<Tarifa>, ITarifaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<TarifaRepository> _logger;

        public TarifaRepository(SGHRContext context,
                                ILogger<TarifaRepository> logger,
                                ILogger<BaseRepository<Tarifa>> loggerBase) : base(context, loggerBase)
        {
            _context = context;
            _logger = logger;
        }

        public override async Task<OperationResult<Tarifa>> SaveAsync(Tarifa entity, int? sesionId = null)
        {
            try
            {
                var result = await base.SaveAsync(entity, sesionId);
                _logger.LogInformation("Se guardó la tarifa con ID {Id}", result.Data?.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando tarifa");
                return OperationResult<Tarifa>.Fail("Ocurrió un error al guardar la tarifa");
            }
        }

        public override async Task<OperationResult<Tarifa>> UpdateAsync(Tarifa entity, int? sesionId = null)
        {
            try
            {
                var result = await base.UpdateAsync(entity, sesionId);
                _logger.LogInformation("Se actualizó la tarifa con ID {Id}", result.Data?.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando tarifa");
                return OperationResult<Tarifa>.Fail("Ocurrió un error al actualizar la tarifa");
            }
        }

        public override async Task<OperationResult<Tarifa>> DeleteAsync(Tarifa entity, int? sesionId = null)
        {
            try
            {
                var result = await base.DeleteAsync(entity, sesionId);
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

        public override async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Tarifa, bool>> filter, bool includeDeleted = false)
        {
            try
            {
                var result = await base.ExistsAsync(filter, includeDeleted);
                if (!result.Success)
                {
                    _logger.LogWarning("No se pudo confirmar la existencia de la tarifa.");
                    return result;
                }

                _logger.LogInformation("Se verifico que la tarifa exite correctamente.");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error al verificar existencia de la tarifa.");
                return OperationResult<bool>.Fail("Ocurrio un error al verificar la existencia de la tarifa.");
            }
        }
    }
}
