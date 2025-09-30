using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reservas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Reservas
{
    public class TarifaRepository : BaseRepository<Tarifa>, ITarifaRepository
    {
        public TarifaRepository(SGHRContext context) : base(context)
        {
        }
    }
}
