using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Repository
{
    public interface IReservaRepository : IBaseRepository<Reserva>
    {
        public Task<OperationResult<List<Reserva>>> GetReservasActivasAsync();
    }
}
