using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Interfaces.Reservas
{
    public interface ITarifaRepository : IBaseRepository<Tarifa>
    {
        public Task<OperationResult<List<Tarifa>>> GetByTemporadaAsync(string temporada);
    }
}
