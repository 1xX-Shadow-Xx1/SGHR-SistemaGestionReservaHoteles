using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.EF.Habitaciones
{
    public sealed class AmenityRepository : BaseRepository<Amenities>, IAmenityRepository
    {
        public AmenityRepository(SGHRContext context)
            : base(context)
        {
        }
    }
}
