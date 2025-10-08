using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Validators.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<AmenityRepository> _logger;
        private readonly IConfiguration _configuration;

        public AmenityRepository(SGHRContext context,
                                   ILogger<AmenityRepository> logger,
                                   IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }
        public override async Task<OperationResult<Amenity>> Save(Amenity entity)
        {
            var result = AmenitiesValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Amenity registrado: {Id} - {Nombre}", entity.ID, entity.Nombre);
            else
                _logger.LogError("Error al registrar Amenity: {Message}", result.Message);

            return result;
        }

        public override async Task<OperationResult<Amenity>> Update(Amenity entity)
        {
            var result = AmenitiesValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Amenity actualizado: {Id}", entity.ID);
            else
                _logger.LogError("Error al actualizar Amenity {Id}: {Message}", entity.ID, result.Message);

            return result;
        }

        public override async Task<OperationResult<Amenity>> Delete(Amenity entity)
        {
            var result = AmenitiesValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            if (result.Success)
                _logger.LogInformation("Amenity eliminado correctamente: {Id}", entity.ID);
            else
                _logger.LogError("Error al eliminar Amenity {Id}: {Message}", entity.ID, result.Message);

            return result;
        }

        public override async Task<OperationResult<Amenity>> GetById(int id)
        {
            try
            {
                var entity = await _context.Amenity
                    .Include(a => a.Categorias) 
                    .FirstOrDefaultAsync(a => a.ID == id && !a.is_deleted);

                if (entity == null)
                    return OperationResult<Amenity>.Fail("Amenity no encontrado");

                return OperationResult<Amenity>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Amenity por Id {Id}", id);
                return OperationResult<Amenity>.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<OperationResult<List<Amenity>>> GetActivosAsync()
        {
            try
            {
                var lista = await _context.Amenity
                    .Where(a => !a.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Amenity>>.Fail("No hay amenities activos");

                return OperationResult<List<Amenity>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener amenities activos");
                return OperationResult<List<Amenity>>.Fail($"Error: {ex.Message}");
            }
        }
        public async Task<OperationResult<Amenity>> GetByNombreAsync(string nombre)
        {
            try
            {
                var entity = await _context.Amenity
                    .FirstOrDefaultAsync(a => a.Nombre == nombre && !a.is_deleted);

                if (entity == null)
                    return OperationResult<Amenity>.Fail("Amenity no encontrado con ese nombre");

                return OperationResult<Amenity>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Amenity por nombre {Nombre}", nombre);
                return OperationResult<Amenity>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
