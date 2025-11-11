
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Enum.Operaciones;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Operaciones;

namespace SGHR.Application.Services.Operaciones
{
    public class MantenimientoServcices : IMantenimientoServices
    {
        private readonly ILogger<MantenimientoServcices> _logger;
        private readonly IMantenimientoRepository _mantenimientoRepo;
        private readonly IHabitacionRepository _habitacionRepo;
        private readonly IReservaRepository _reservaRepo;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly IPisoRepository _pisoRepo;

        public MantenimientoServcices(ILogger<MantenimientoServcices> logger,
                                      IMantenimientoRepository mantenimientoRepo,
                                      IHabitacionRepository habitacionRepository,
                                      IReservaRepository reservaRepository,
                                      IUsuarioRepository suarioRepo,
                                      IPisoRepository pisoRepository)
        {
            _logger = logger;
            _mantenimientoRepo = mantenimientoRepo;
            _habitacionRepo = habitacionRepository;
            _reservaRepo = reservaRepository;
            _usuarioRepo = suarioRepo;
            _pisoRepo = pisoRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateMantenimientoDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "El mantenimiento no puede ser nulo.";
                return result;
            }
            try
            {
                var ListReserva = await _reservaRepo.GetAllAsync();
                if (!ListReserva.Success)
                {
                    result.Message = ListReserva.Message;
                    return result;
                }
                var ListMantenimiento = await _mantenimientoRepo.GetAllAsync();
                if (!ListMantenimiento.Success)
                {
                    result.Message = ListMantenimiento.Message;
                    return result;
                }

                var ListHabitaciones = await _habitacionRepo.GetAllAsync();
                if (!ListMantenimiento.Success)
                {
                    result.Message = ListMantenimiento.Message;
                    return result;
                }
                               
                
                
                var ExistPiso = await _pisoRepo.GetByNumeroPisoAsync(CreateDto.NumeroPiso);

                if (!ExistPiso.Success)
                {
                    result.Message = ExistPiso.Message;
                    return result;
                }                           


                var ExistHabitacion = ListHabitaciones.Data.FirstOrDefault(u => u.Numero == CreateDto.NumeroHabitacion);
                if (ExistHabitacion == null)
                {
                    result.Message = $"No se encontro la habitacion, introduce el numero de una habitacion ya correctamente.";
                    return result;
                }

                var ExistReserva = ListReserva.Data.FirstOrDefault(u => u.IdHabitacion == ExistHabitacion.Id);
                if (ExistHabitacion.Estado == EstadoHabitacion.Reservada)
                {
                    result.Message = $"La habitacion ya a sido reservada tendra que cancelar primero la reserva para poder hacer el mantenimiento. ID de la reserva: {ExistReserva.Id}";
                    return result;
                }

                var ExisUser = await _usuarioRepo.GetByCorreoAsync(CreateDto.RealizadoPor);
                if (!ExisUser.Success)
                {
                    result.Message = ExisUser.Message;
                    return result;
                }
                

                Mantenimiento mantenimiento = new Mantenimiento()
                {
                    IdHabitacion = ExistHabitacion.Id,
                    RealizadoPor = ExisUser.Data.Id,
                    IdPiso = ExistPiso.Data.Id,
                    Descripcion = CreateDto.Descripcion,
                    FechaInicio = CreateDto.FechaInicio == null ? CreateDto.FechaInicio : DateTime.Now
                };

                var opResult = await _mantenimientoRepo.SaveAsync(mantenimiento);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ExistHabitacion.Estado = EstadoHabitacion.Mantenimiento;
                var opHabit = await _habitacionRepo.UpdateAsync(ExistHabitacion);
                if (!opHabit.Success)
                {
                    result.Message = $"Error al actualizar el estado de la habitacion: {opHabit.Message}";
                    return result;
                }

                MantenimientoDto MantenimientoDto = new MantenimientoDto()
                {
                    Id = opResult.Data.Id,
                    RealizadoPor = ExisUser.Data.Correo,
                    NumeroHabitacion = ExistHabitacion.Numero,
                    NumeroPiso = CreateDto.NumeroPiso,
                    Descripcion = CreateDto.Descripcion,
                    Estado = opResult.Data.Estado,
                    FechaInicio = opResult.Data.FechaInicio,
                    FechaFin = opResult.Data.FechaFin
                };

                result.Success = true;
                result.Data = MantenimientoDto;
                result.Message = $"Se a registrado el mantenimiento correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al registrar el mantenimiento: {ex.Message}";
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
                var existMantenimiento = await _mantenimientoRepo.GetByIdAsync(id);
                if (!existMantenimiento.Success)
                {
                    result.Message = existMantenimiento.Message;
                    return result;
                }

                var OpResult = await _mantenimientoRepo.DeleteAsync(existMantenimiento.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Mantenimiento con id {existMantenimiento.Data.Id} eliminada correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar el mantenimiento: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListMantenimiento = await _mantenimientoRepo.GetAllAsync();
                if (!ListMantenimiento.Success)
                {
                    result.Message = ListMantenimiento.Message;
                    return result;
                }

                var ListHabitaciones = await _habitacionRepo.GetAllAsync();
                if (!ListHabitaciones.Success)
                {
                    result.Message = ListHabitaciones.Message;
                    return result;
                }

                var ListPiso = await _pisoRepo.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }                

                var ListaUsuarios = await _usuarioRepo.GetAllAsync();
                if (!ListaUsuarios.Success)
                {
                    result.Message = ListaUsuarios.Message;
                    return result;
                }

                var reporteDtos = (
                    from m in ListMantenimiento.Data
                    join u in ListaUsuarios.Data on m.RealizadoPor equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty()

                    join h in ListHabitaciones.Data on m.IdHabitacion equals h.Id into habGroup
                    from h in habGroup.DefaultIfEmpty()

                    join p in ListPiso.Data on m.IdPiso equals p.Id into pisoGroup
                    from p in pisoGroup.DefaultIfEmpty()

                    select new MantenimientoDto
                    {
                        Id = m.Id,
                        NumeroHabitacion = h.Numero,
                        NumeroPiso = p.NumeroPiso,
                        Descripcion = m.Descripcion,
                        RealizadoPor = u.Correo,
                        Estado = m.Estado,
                        FechaInicio = m.FechaInicio,
                        FechaFin = m.FechaFin
                    }
                ).ToList();

                result.Success = true;
                result.Data = reporteDtos;
                result.Message = "Se obtuvieron los mantenimientos correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Erro al obtener los mantenimientos.";
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
                var ExistMantenimiento = await _mantenimientoRepo.GetByIdAsync(id);
                if (!ExistMantenimiento.Success)
                {
                    result.Message = ExistMantenimiento.Message;
                    return result;
                }

                var ExistHabitacion = await _habitacionRepo.GetByIdAsync(ExistMantenimiento.Data.IdHabitacion);
                if (!ExistHabitacion.Success)
                {
                    result.Message = ExistHabitacion.Message;
                    return result;
                }

                var ExistPiso = await _pisoRepo.GetByIdAsync(ExistMantenimiento.Data.IdPiso);
                if (!ExistPiso.Success)
                {
                    result.Message = ExistPiso.Message;
                    return result;
                }                

                var ExistUsuarios = await _usuarioRepo.GetByIdAsync(ExistMantenimiento.Data.RealizadoPor);
                if (!ExistUsuarios.Success)
                {
                    result.Message = ExistUsuarios.Message;
                    return result;
                }

                MantenimientoDto mantenimientoDto = new MantenimientoDto()
                {
                    Id = ExistMantenimiento.Data.Id,
                    NumeroHabitacion = ExistHabitacion.Data.Numero,
                    NumeroPiso = ExistPiso.Data.NumeroPiso,
                    Descripcion = ExistMantenimiento.Data.Descripcion,
                    RealizadoPor = ExistUsuarios.Data.Correo,
                    Estado = ExistMantenimiento.Data.Estado,
                    FechaInicio = ExistMantenimiento.Data.FechaInicio,
                    FechaFin = ExistMantenimiento.Data.FechaFin
                };

                result.Success = true;
                result.Data = mantenimientoDto;
                result.Message = $"Se obtuvo el mantenimiento con id {id} correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener el mantenimiento: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UpdateMantenimientoDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El mantenimiento no puede ser nulo.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var mantenimiento = await _mantenimientoRepo.GetByIdAsync(UpdateDto.Id);
                if (!mantenimiento.Success)
                {
                    result.Message = mantenimiento.Message;
                    return result;
                }

                var ListHabitacion = await _habitacionRepo.GetAllAsync();
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }
                var ExistHabitacion = ListHabitacion.Data.FirstOrDefault(h => h.Numero == UpdateDto.NumeroHabitacion);
                if(ExistHabitacion == null)
                {
                    result.Message = $"No existe la habitacion a la cual quiere actualizar el mantenimiento.";
                    return result;
                }

                var ExistPiso = await _pisoRepo.GetByNumeroPisoAsync(UpdateDto.NumeroPiso);
                if (!ExistPiso.Success)
                {
                    result.Message = ExistPiso.Message;
                    return result;
                }

                var ExistUsuarios = await _usuarioRepo.GetByCorreoAsync(UpdateDto.RealizadoPor);
                if (!ExistUsuarios.Success)
                {
                    result.Message = ExistUsuarios.Message;
                    return result;
                }

                mantenimiento.Data.RealizadoPor = ExistUsuarios.Data.Id;
                mantenimiento.Data.IdHabitacion = ExistHabitacion.Id;
                mantenimiento.Data.IdPiso = ExistPiso.Data.Id;
                mantenimiento.Data.Descripcion = UpdateDto.Descripcion;
                mantenimiento.Data.FechaInicio = UpdateDto.FechaInicio;
                mantenimiento.Data.FechaFin = UpdateDto.FechaFin;                
                mantenimiento.Data.Estado = UpdateDto.Estado;
                

                var opResult = await _mantenimientoRepo.UpdateAsync(mantenimiento.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                MantenimientoDto mantenimientoDto = new MantenimientoDto()
                {
                    Id = mantenimiento.Data.Id,
                    NumeroHabitacion = ExistHabitacion.Numero,
                    NumeroPiso = ExistPiso.Data.NumeroPiso,
                    Descripcion = mantenimiento.Data.Descripcion,
                    RealizadoPor = ExistUsuarios.Data.Correo,
                    Estado = mantenimiento.Data.Estado,
                    FechaInicio = mantenimiento.Data.FechaInicio,
                    FechaFin = mantenimiento.Data.FechaFin
                };

                result.Success = true;
                result.Data = mantenimientoDto;
                result.Message = $"Se a actualizado el mantenimiento correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al actualiza el mantenimiento: {ex.Message}";
            }
            return result;
        }
    }
}
