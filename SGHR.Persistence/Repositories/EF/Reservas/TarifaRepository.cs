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
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Tarifa>> Update(Tarifa entity)
        {
            var result = TarifaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Update(entity);
        }

        public override async Task<OperationResult<Tarifa>> Delete(Tarifa entity)
        {
            var result = TarifaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Delete(entity);
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
        public async Task<OperationResult<List<Tarifa>>> GetAll()
        {
            var result = await base.GetAll();
            if (result.Success)
                _logger.LogInformation("Tarifas obtenidas correctamente.");
            else
                _logger.LogError("Error al obtener las Tarifas: {Message}", result.Message);
            return result;
        }
        public async Task<OperationResult<List<Tarifa>>> GetByTemporadaAsync(string temporada)
        {
            try
            {
                var tarifas = await _context.Tarifa
                    .Where(t => t.Temporada == temporada && !t.is_deleted)
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
