using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Interfaces.Reservas
{
    public interface IServicioAdicionalService : IBaseServices<CreateServicioAdicionalDto, UpdateServicioAdicionalDto>
    {
    }
}
