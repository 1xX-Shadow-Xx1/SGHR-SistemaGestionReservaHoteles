using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Operaciones
{
    public class MantenimientoRepository : BaseRepository<Mantenimiento>, IMantenimientoRepository
    {
        public MantenimientoRepository(SGHRContext context) : base(context)
        {
        }
    }
}
