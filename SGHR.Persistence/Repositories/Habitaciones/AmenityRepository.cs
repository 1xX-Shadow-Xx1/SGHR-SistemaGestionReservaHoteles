using SchoolPoliApp.Persistence.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Contex;
using SGHR.Persistence.Interfaces.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Persistence.Repositories.Habitaciones
{
    public class AmenityRepository : BaseRepository<Amenities>, IAmenitiesRepository
    {
        public AmenityRepository(SGHRContext context)
            : base(context)
        {
        }
    }
}
