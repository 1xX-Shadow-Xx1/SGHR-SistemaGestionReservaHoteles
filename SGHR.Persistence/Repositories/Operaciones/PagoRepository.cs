using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Operaciones
{
    public class PagoRepository : BaseRepository<Pago>, IPagoRepository
    {
        public PagoRepository(SGHRContext context) : base(context)
        {
        }
    }
}
