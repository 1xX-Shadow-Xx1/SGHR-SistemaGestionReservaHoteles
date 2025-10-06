using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;


namespace SGHR.Application.Interfaces.Operaciones
{
    public interface IAuditoryService : IBaseServices<CreateAuditoryDto, UpdateAuditoryDto, DeleteAuditoryDto>
    {
    }
}
