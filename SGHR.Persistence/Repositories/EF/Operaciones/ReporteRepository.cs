using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Reportes;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Operaciones
{
    public sealed class ReporteRepository : BaseRepository<Reporte>, IReporteRepository
    {
        public ReporteRepository(SGHRContext context) : base(context)
        {
        }
    }
}
