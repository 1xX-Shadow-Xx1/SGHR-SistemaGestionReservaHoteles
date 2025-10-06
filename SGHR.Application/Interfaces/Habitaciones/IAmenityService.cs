
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;

namespace SGHR.Application.Interfaces.Habitaciones
{
    public interface IAmenityService : IBaseServices<CreateAmenityDto, UpdateAmenityDto, DeleteAmenityDto>
    {
    }
}
