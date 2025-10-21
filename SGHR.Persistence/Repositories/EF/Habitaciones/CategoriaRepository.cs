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
    public sealed class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<CategoriaRepository> _logger;
        private readonly IConfiguration _configuration;

        public CategoriaRepository(SGHRContext context,
                                   ILogger<CategoriaRepository> logger,
                                   IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Categoria>> Save(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Save(entity);
        }

        public override async Task<OperationResult<Categoria>> Update(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            return await base.Update(entity);
        }

        public override async Task<OperationResult<Categoria>> Delete(Categoria entity)
        {
            var result = CategoriaValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }

            return await base.Delete(entity);
        }

        public override async Task<OperationResult<Categoria>> GetById(int id)
        {
            try
            {
                var entity = await _context.Categoria
                    .FirstOrDefaultAsync(C => C.ID == id && !C.is_deleted);

                if (entity == null)
                    return OperationResult<Categoria>.Fail("Categoria no encontrada");

                return OperationResult<Categoria>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Categoria por Id {Id}", id);
                return OperationResult<Categoria>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<List<Categoria>>> GetActivasAsync()
        {
            try
            {
                var lista = await _context.Categoria
                    .Where(c => !c.is_deleted)
                    .ToListAsync();

                if (!lista.Any())
                    return OperationResult<List<Categoria>>.Fail("No hay categorías activas");

                return OperationResult<List<Categoria>>.Ok(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener categorías activas");
                return OperationResult<List<Categoria>>.Fail($"Error: {ex.Message}");
            }
        }

        public async Task<OperationResult<Categoria>> GetByNombreAsync(string nombre)
        {
            try
            {
                var entity = await _context.Categoria
                    .FirstOrDefaultAsync(c => c.Nombre == nombre && !c.is_deleted);

                if (entity == null)
                    return OperationResult<Categoria>.Fail("Categoría no encontrada con ese nombre");

                return OperationResult<Categoria>.Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener Categoría por nombre {Nombre}", nombre);
                return OperationResult<Categoria>.Fail($"Error: {ex.Message}");
            }
        }
    }
}
