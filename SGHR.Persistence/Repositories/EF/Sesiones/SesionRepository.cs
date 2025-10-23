using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Sesiones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Sesiones;
using System.ClientModel.Primitives;
using System.Linq.Expressions;

namespace SGHR.Persistence.Repositories.EF.Sesiones
{
    public class SesionRepository : BaseRepository<Sesion>, ISesionRepository
    {
        private readonly ILogger<SesionRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly SGHRContext _context;

        public SesionRepository(SGHRContext context,
                                ILogger<SesionRepository> logger,
                                IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<Sesion>> Save(Sesion entity)
        {
            OperationResult<Sesion> result = new OperationResult<Sesion>();
            _logger.LogInformation("Iniciando el guardado de la Sesion");

            try
            {
                var OpResult = await base.Save(entity);
                if(!OpResult.Success)
                {
                    _logger.LogInformation("Error al Guardar la Sesion: " + OpResult.Message);
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }
                _logger.LogInformation($"Sesion Guardad Correctamente.");
                result = OpResult;

            }catch (Exception ex)
            {
                _logger.LogInformation($"Error al guardar: {ex.Message}");
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public override async Task<OperationResult<Sesion>> Update(Sesion entity)
        {
            OperationResult<Sesion> result = new OperationResult<Sesion>();
            _logger.LogInformation("Iniciando con la actualizacion de la Sesion.");

            try
            {
                var OpResult = await base.Update(entity);
                if(!OpResult.Success)
                {
                    _logger.LogInformation($"Error al actualizar: {OpResult.Message}");
                    result.Success = false;
                    result.Message = OpResult.Message;
                    return result;
                }
                _logger.LogInformation($"Sesion actualizadad correctamente.");
                result = OpResult;

            }catch (Exception ex)
            {
                _logger.LogInformation($"Error al actualizar: {ex.Message}");
                result.Success = false;
                result.Message = ($"Error al actualizar: {ex.Message}");
            }
            return result;
        }

        public async Task<OperationResult<Sesion>> Delete(Sesion entity)
        {
            throw new NotImplementedException();
        }

        public override async Task<OperationResult<Sesion>> GetById(int id)
        {
            OperationResult<Sesion> result = new OperationResult<Sesion>();
            _logger.LogInformation($"Iniciando Optencion de la Sesion con el ID: {id}");

            try
            {
                var OResult = await base.GetById(id);
                if(!OResult.Success)
                {
                    _logger.LogInformation($"Error con la obtencion del ID: {OResult.Message}");
                    result.Success = false;
                    result.Message = OResult.Message;
                    return result;
                }
                _logger.LogInformation($"Sesion con ID {id}, obtenida exitosamente ");
                result = OResult;

            }catch (Exception ex)
            {
                _logger.LogInformation($"Error de Excepcion: {ex.Message}");
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<OperationResult<bool>> ExistsAsync(Expression<Func<Sesion, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public override async Task<OperationResult<List<Sesion>>> GetAll()
        {
            var result = await base.GetAll();
            return result;
        }

        public async Task<OperationResult<List<Sesion>>> GetAllBY(Expression<Func<Sesion, bool>> filter)
        {
            return await base.GetAllBY(filter);
        }        
    }
}
