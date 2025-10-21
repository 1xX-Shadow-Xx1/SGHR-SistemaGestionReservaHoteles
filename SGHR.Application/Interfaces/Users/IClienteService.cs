using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Interfaces.Users
{
    public interface IClienteService : IBaseServices<CreateClienteDto, UpdateClienteDto>
    {
    }
}
