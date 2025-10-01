using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Contex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.ADO.Reservas
{
    public sealed class ReservaRepositoryADO : BaseRepository<Reserva>, IReservaRepository
    {
        public ReservaRepositoryADO(SGHRContext context) : base(context)
        {
        }

        public Task<OperationResult<List<Reserva>>> GetReservasActivasAsync()
        {
            throw new NotImplementedException();
        }
    }
}
