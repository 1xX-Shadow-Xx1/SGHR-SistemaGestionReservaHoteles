
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.Services.Reservas
{
    public class ReservaServices : IReservaServices
    {
        private readonly ILogger<ReservaServices> _logger;
        private readonly IReservaRepository _reservarepository;
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ReservaServices(ILogger<ReservaServices> logger, 
                               IReservaRepository repository,
                               IUsuarioRepository suarioRepository,
                               IHabitacionRepository habitacionRepository,
                               IClienteRepository clienteRepository)
        {
            _logger = logger;
            _reservarepository = repository;
            _usuarioRepository = suarioRepository;
            _clienteRepository = clienteRepository;
            _habitacionRepository = habitacionRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateReservaDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "La habitacion no puede ser nula.";
                return result;
            }
            try
            {
                var ListHabitacion = await _habitacionRepository.GetAvailableAsync(CreateDto.FechaInicio, CreateDto.FechaFin);
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }

                if(ListHabitacion.Data.Count() == 0)
                {
                    result.Message = "No hay habitaciones disponibles para esas fecha.";
                    return result;
                }

                var ExistHabitacion = ListHabitacion.Data.FirstOrDefault(h => h.Numero == CreateDto.NumeroHabitacion);
                if (ExistHabitacion == null)
                {
                    result.Message = "No hay una habitacion disponible con ese numero.";
                    return result;
                }

                var ListClientes = await _clienteRepository.GetAllAsync();
                if (!ListClientes.Success)
                {
                    result.Message = ListClientes.Message;
                    return result;
                }

                var Cliente = ListClientes.Data.FirstOrDefault(p => p.Cedula == CreateDto.CedulaCliente);
                if (Cliente == null)
                {
                    result.Message = "No existe un cliente registrado con ese numero de cedula.";
                    return result;
                }
                               

                Reserva reserva = new Reserva()
                {
                    IdCliente = Cliente.Id,
                    IdHabitacion = ExistHabitacion.Id,
                    FechaInicio = CreateDto.FechaInicio,
                    FechaFin = CreateDto.FechaFin,
                    CostoTotal = CreateDto.CostoTotal
                };

                if (!string.IsNullOrWhiteSpace(CreateDto.CorreoCliente))
                {
                    var ListUsuarios = await _usuarioRepository.GetAllAsync();
                    if (!ListUsuarios.Success)
                    {
                        result.Message = ListUsuarios.Message;
                    }

                    var Usuario = ListUsuarios.Data.FirstOrDefault(p => p.Correo == CreateDto.CorreoCliente);
                    if (Usuario == null)
                    {
                        result.Message = "No existe un Usuario registrado con ese correo.";
                        return result;
                    }

                    reserva.IdUsuario = Usuario.Id;
                }

                var opResult = await _reservarepository.SaveAsync(reserva);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ReservaDto reservaDto = new ReservaDto()
                {
                    Id = opResult.Data.Id,
                    CedulaCliente = Cliente.Cedula,
                    NumeroHabitacion = ExistHabitacion.Numero,
                    CorreoCliente = CreateDto.CorreoCliente,
                    FechaInicio = CreateDto.FechaInicio,
                    FechaFin = CreateDto.FechaFin,
                    CostoTotal = CreateDto.CostoTotal,
                    Estado = opResult.Data.Estado.ToString(),
                };

                result.Success = true;
                result.Message = "Se a registrado la reserva correctamente.";
                result.Data = reservaDto;


            }
            catch (Exception ex)
            {
                result.Message = $"Error creado la reserva: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var existReserva = await _reservarepository.GetByIdAsync(id);
                if (!existReserva.Success)
                {
                    result.Message = $"No existe una reserva con ese id.";
                    return result;
                }

                var reserva = existReserva.Data;
                if(reserva.Estado == EstadoReserva.Activa || reserva.Estado == EstadoReserva.Confirmada)
                {
                    result.Message = "No se puede eliminar una reserva que este activa o confirmada.";
                    return result;
                }

                var OpResult = await _reservarepository.DeleteAsync(reserva);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Reserva con id {existReserva.Data.Id} eliminada correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error eliminando la reserva: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListHabitacion = await _habitacionRepository.GetAllAsync();
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }
                var ListReservas = await _reservarepository.GetAllAsync();
                if (!ListReservas.Success)
                {
                    result.Message = ListReservas.Message;
                    return result;
                }
                var ListClientes = await _clienteRepository.GetAllAsync();
                if (!ListClientes.Success)
                {
                    result.Message = ListClientes.Message;
                    return result;
                }
                var ListUsuarios = await _usuarioRepository.GetAllAsync();
                if (!ListUsuarios.Success)
                {
                    result.Message = ListUsuarios.Message;
                }


                var ReservaDtos = (
                    from r in ListReservas.Data

                    join h in ListHabitacion.Data on r.IdHabitacion equals h.Id into habGroup
                    from hab in habGroup.DefaultIfEmpty()

                    join c in ListClientes.Data on r.IdCliente equals c.Id into clientGroup
                    from cli in clientGroup.DefaultIfEmpty()

                    join u in ListUsuarios.Data on r.IdUsuario equals u.Id into userGroup
                    from usr in userGroup.DefaultIfEmpty()

                    select new ReservaDto
                    {
                        Id = r.Id,
                        FechaInicio = r.FechaInicio,
                        FechaFin = r.FechaFin,
                        Estado = r.Estado.ToString(),
                        NumeroHabitacion = hab.Numero,
                        CedulaCliente = cli.Cedula,
                        CorreoCliente = usr.Correo != null ? usr.Nombre : "N/A"
                    }
                ).ToList();



                result.Success = true;
                result.Data = ReservaDtos;
                result.Message = "Se obtuvieron las reservas correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error obteniendo las reservas: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var Habitacion = await _habitacionRepository.GetByIdAsync(id);
                if (!Habitacion.Success)
                {
                    result.Message = Habitacion.Message;
                    return result;
                }
                var reserva = await _reservarepository.GetByIdAsync(Habitacion.Data.IdCategoria);
                if (!reserva.Success)
                {
                    result.Message = reserva.Message;
                    return result;
                }
                var cliente = await _clienteRepository.GetByIdAsync(Habitacion.Data.IdPiso);
                if (!cliente.Success)
                {
                    result.Message = cliente.Message;
                    return result;
                }
                var usuario = await _usuarioRepository.GetByIdAsync(Habitacion.Data.IdPiso);
                if (!usuario.Success)
                {
                    result.Message = usuario.Message;
                    return result;
                }

                ReservaDto ReservaDto = new ReservaDto()
                {
                    Id = id,
                    CedulaCliente = cliente.Data.Cedula,
                    NumeroHabitacion = Habitacion.Data.Numero,
                    CorreoCliente = usuario.Data.Correo,
                    FechaInicio = reserva.Data.FechaInicio,
                    FechaFin = reserva.Data.FechaFin,
                    CostoTotal = reserva.Data.CostoTotal,
                    Estado = Habitacion.Data.Estado.ToString()
                };

                result.Success = true;
                result.Data = ReservaDto;
                result.Message = "Se obtuvo la reserva correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener la reserva por id: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateReservaDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "La habitacion no puede ser nula.";
                return result;
            }
            if(UpdateDto.Id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var reserva = await _reservarepository.GetByIdAsync(UpdateDto.Id);
                if (!reserva.Success)
                {
                    result.Message = "No se encontro una reserva con ese id.";
                    return result;
                }
                if(reserva.Data.Estado == EstadoReserva.Finalizada)
                {
                    result.Message = "No se puede actualizar una reserva que ya este finalizada.";
                    return result;
                }

                var ListHabitacion = await _habitacionRepository.GetAvailableAsync(UpdateDto.FechaInicio, UpdateDto.FechaFin, UpdateDto.Id);
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }

                if (ListHabitacion.Data.Count() == 0)
                {
                    result.Message = "No hay habitaciones disponibles para esas fecha.";
                    return result;
                }

                var ExistHabitacion = ListHabitacion.Data.FirstOrDefault(h => h.Numero == UpdateDto.NumeroHabitacion);
                if (ExistHabitacion == null)
                {
                    result.Message = "No hay una habitacion disponible con ese numero.";
                    return result;
                }

                var ListClientes = await _clienteRepository.GetAllAsync();
                if (!ListClientes.Success)
                {
                    result.Message = ListClientes.Message;
                    return result;
                }

                var Cliente = ListClientes.Data.FirstOrDefault(p => p.Cedula == UpdateDto.CedulaCliente);
                if (Cliente == null)
                {
                    result.Message = "No existe un cliente registrado con ese numero de cedula.";
                    return result;
                }

                reserva.Data.IdCliente = Cliente.Id;
                reserva.Data.IdHabitacion = ExistHabitacion.Id;
                reserva.Data.FechaInicio = UpdateDto.FechaInicio;
                reserva.Data.FechaFin = UpdateDto.FechaFin;
                reserva.Data.CostoTotal = UpdateDto.CostoTotal;
                if( Enum.TryParse<EstadoReserva>(UpdateDto.Estado, out var estado))
                {
                    reserva.Data.Estado = estado;
                }

                if (!string.IsNullOrWhiteSpace(UpdateDto.CorreoCliente))
                {
                    var ListUsuarios = await _usuarioRepository.GetAllAsync();
                    if (!ListUsuarios.Success)
                    {
                        result.Message = ListUsuarios.Message;
                    }

                    var Usuario = ListUsuarios.Data.FirstOrDefault(p => p.Correo == UpdateDto.CorreoCliente);
                    if (Usuario == null)
                    {
                        result.Message = "No existe un Usuario registrado con ese correo.";
                        return result;
                    }

                    reserva.Data.IdUsuario = Usuario.Id;
                }

                var opResult = await _reservarepository.SaveAsync(reserva.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ReservaDto reservaDto = new ReservaDto()
                {
                    Id = opResult.Data.Id,
                    CedulaCliente = Cliente.Cedula,
                    NumeroHabitacion = ExistHabitacion.Numero,
                    CorreoCliente = UpdateDto.CorreoCliente,
                    FechaInicio = UpdateDto.FechaInicio,
                    FechaFin = UpdateDto.FechaFin,
                    CostoTotal = UpdateDto.CostoTotal,
                    Estado = opResult.Data.Estado.ToString(),
                };

                result.Success = true;
                result.Message = "Se a actualizado la reserva correctamente.";
                result.Data = reservaDto;


            }
            catch (Exception ex)
            {
                result.Message = $"Error actualizando la reserva: {ex.Message}";
            }
            return result;
        }
    }
}
