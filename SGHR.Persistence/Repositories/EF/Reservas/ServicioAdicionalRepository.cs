using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Validators.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Reservas
{
    public sealed class ServicioAdicionalRepository : BaseRepository<ServicioAdicional>, IServicioAdicionalRepository
    {
        private readonly SGHRContext _context;
        private readonly ILogger<ServicioAdicionalRepository> _logger;
        private readonly IConfiguration _configuration;

        public ServicioAdicionalRepository(SGHRContext context,
                                           ILogger<ServicioAdicionalRepository> logger,
                                           IConfiguration configuration) : base(context)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public override async Task<OperationResult<ServicioAdicional>> Save(ServicioAdicional entity)
        {
            var result = ServicioAdicionalValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Save(entity);
        }

        public override async Task<OperationResult<ServicioAdicional>> Update(ServicioAdicional entity)
        {
            var result = ServicioAdicionalValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Update(entity);
        }

        public override async Task<OperationResult<ServicioAdicional>> Delete(ServicioAdicional entity)
        {
            var result = ServicioAdicionalValidator.Validate(entity);
            if (!result.Success)
            {
                return result;
            }
            return await base.Delete(entity);
        }

        public override async Task<OperationResult<ServicioAdicional>> GetById(int id)
        {
            var result = await base.GetById(id);

            if (result.Success)
                _logger.LogInformation("Servicio adicional encontrado: {Id}", id);
            else
                _logger.LogWarning("No se encontró Servicio adicional con Id {Id}", id);

            return result;
        }
    }
}
