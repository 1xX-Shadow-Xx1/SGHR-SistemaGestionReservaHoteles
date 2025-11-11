
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Enum.Habitaciones;
using SGHR.Domain.Enum.Reservas;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Reservas;
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
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IServicioAdicionalRepository _servicioAdicionalRepository;
        private readonly IAmenityRepository _amenityRepository;

        public ReservaServices(ILogger<ReservaServices> logger, 
                               IReservaRepository repository,
                               IUsuarioRepository suarioRepository,
                               IHabitacionRepository habitacionRepository,
                               IClienteRepository clienteRepository,
                               ITarifaRepository tarifaRepository,
                               ICategoriaRepository categoriaRepository,
                               IServicioAdicionalRepository servicioAdicionalRepository,
                               IAmenityRepository amenityRepository)
                            
        {
            _logger = logger;
            _reservarepository = repository;
            _usuarioRepository = suarioRepository;
            _clienteRepository = clienteRepository;
            _habitacionRepository = habitacionRepository;
            _tarifaRepository = tarifaRepository;
            _categoriaRepository = categoriaRepository;
            _servicioAdicionalRepository = servicioAdicionalRepository;
            _amenityRepository = amenityRepository;

        }

        public async Task<ServiceResult> CreateAsync(CreateReservaDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "La reserva no puede ser nula.";
                return result;
            }
            try
            {
                if (CreateDto.FechaInicio >= CreateDto.FechaFin)
                {
                    result.Message = "Las fechas de la reserva son inválidas.";
                    return result;
                }

                var ListHabitacion = await _habitacionRepository.GetAvailableAsync(CreateDto.FechaInicio, CreateDto.FechaFin);
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

                var categoria = await _categoriaRepository.GetByIdAsync(ExistHabitacion.IdCategoria);
                if (!categoria.Success)
                {
                    result.Message = categoria.Message;
                    return result;
                }

                var listTarifaByCategory = await _tarifaRepository.GetByCategoriaAsync(categoria.Data.Id);
                if (!listTarifaByCategory.Success)
                {
                    result.Message = listTarifaByCategory.Message;
                    return result;
                }

                var amenity = await _amenityRepository.GetByIdAsync(ExistHabitacion.IdAmenity);
                if (!amenity.Success)
                {
                    result.Message = amenity.Message;
                    return result;
                }
                var Coste = amenity.Data.Precio;
                Coste += amenity.Data.PorCapacidad * ExistHabitacion.Capacidad;

                var fecha = DateTime.Now;

                var tarifa = listTarifaByCategory.Data.FirstOrDefault(t => fecha >= t.Fecha_inicio &&  fecha <= t.Fecha_fin);

                if (tarifa == null)
                    Coste += categoria.Data.Precio;
                else
                    Coste += tarifa.Precio;


                Reserva reserva = new Reserva()
                {
                    IdCliente = Cliente.Id,
                    IdHabitacion = ExistHabitacion.Id,
                    FechaInicio = CreateDto.FechaInicio,
                    FechaFin = CreateDto.FechaFin,
                    CostoTotal = Coste != null ? Coste : 0
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

                ExistHabitacion.Estado = EstadoHabitacion.Reservada;

                var ophabitacion = await _habitacionRepository.UpdateAsync(ExistHabitacion);
                if (!ophabitacion.Success)
                {
                    result.Message = ophabitacion.Message;
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
                    CostoTotal = opResult.Data.CostoTotal,
                    Estado = opResult.Data.Estado
                };

                result.Success = true;
                result.Message = "Reserva registrada correctamente.";
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
                    result.Message = $"Reserva no encontrada.";
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

                var habitacion = await _habitacionRepository.GetByIdAsync(reserva.IdHabitacion);
                if (!habitacion.Success)
                {
                    result.Message = habitacion.Message;
                    return result;
                }

                habitacion.Data.Estado = EstadoHabitacion.Disponible;
                var ophabitacion = await _habitacionRepository.UpdateAsync(habitacion.Data);
                if (!ophabitacion.Success)
                {
                    result.Message = ophabitacion.Message;
                    return result;
                }

                result.Success = true;
                result.Message = $"Reserva eliminada exitosamente.";


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
                        Estado = r.Estado,
                        NumeroHabitacion = hab.Numero,
                        CedulaCliente = cli.Cedula,
                        CorreoCliente = usr.Correo != null ? usr.Correo : "N/A",
                        CostoTotal = r.CostoTotal
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
                var reserva = await _reservarepository.GetByIdAsync(id);
                if (!reserva.Success)
                {
                    result.Message = reserva.Message;
                    return result;
                }
                var Habitacion = await _habitacionRepository.GetByIdAsync(reserva.Data.IdHabitacion);
                if (!Habitacion.Success)
                {
                    result.Message = Habitacion.Message;
                    return result;
                }                
                var cliente = await _clienteRepository.GetByIdAsync(reserva.Data.IdCliente);
                if (!cliente.Success)
                {
                    result.Message = cliente.Message;
                    return result;
                }
                var usuario = await _usuarioRepository.GetByIdAsync((int)cliente.Data.IdUsuario);
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
                    Estado = reserva.Data.Estado
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
                result.Message = "La reserva no puede ser nula.";
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
                    result.Message = "Reserva no encontrada.";
                    return result;
                }
                if(reserva.Data.Estado == EstadoReserva.Finalizada)
                {
                    result.Message = "No se puede actualizar una reserva que ya este finalizada.";
                    return result;
                }
                if(UpdateDto.FechaInicio >= UpdateDto.FechaFin)
                {
                    result.Message = "Las fechas de la reserva son inválidas.";
                    return result;
                }
                var ListHabitacion = await _habitacionRepository.GetAvailableAsync(UpdateDto.FechaFin, UpdateDto.FechaInicio, UpdateDto.Id);
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

                var categoria = await _categoriaRepository.GetByIdAsync(ExistHabitacion.IdCategoria);
                if (!categoria.Success)
                {
                    result.Message = categoria.Message;
                    return result;
                }

                var listTarifaByCategory = await _tarifaRepository.GetByCategoriaAsync(categoria.Data.Id);
                if (!listTarifaByCategory.Success)
                {
                    result.Message = listTarifaByCategory.Message;
                    return result;
                }

                var amenity = await _amenityRepository.GetByIdAsync(ExistHabitacion.IdAmenity);
                if (!amenity.Success)
                {
                    result.Message = amenity.Message;
                    return result;
                }

                var Coste = amenity.Data.Precio;
                Coste += amenity.Data.PorCapacidad * ExistHabitacion.Capacidad;

                var fecha = DateTime.Now;

                var tarifa = listTarifaByCategory.Data.FirstOrDefault(t => fecha >= t.Fecha_inicio && fecha <= t.Fecha_fin);

                if (tarifa == null)
                    Coste += categoria.Data.Precio;
                else
                    Coste += tarifa.Precio;

                reserva.Data.IdCliente = Cliente.Id;
                reserva.Data.IdHabitacion = ExistHabitacion.Id;
                reserva.Data.FechaInicio = UpdateDto.FechaInicio;
                reserva.Data.FechaFin = UpdateDto.FechaFin;
                reserva.Data.CostoTotal = Coste;
                reserva.Data.Estado = (EstadoReserva)UpdateDto.Estado;

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

                var opResult = await _reservarepository.UpdateAsync(reserva.Data);
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
                    CostoTotal = opResult.Data.CostoTotal,
                    Estado = opResult.Data.Estado,
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
        public async Task<ServiceResult> AddServicioAdicional(int id, string nombreServicio)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ExistServicio = await _servicioAdicionalRepository.GetByNombreAsync(nombreServicio);
                if (!ExistServicio.Success)
                {
                    result.Message = ExistServicio.Message;
                    return result;
                }

                var ExistReserva = await _reservarepository.GetByIdAsync(id);
                if (!ExistReserva.Success)
                {
                    result.Message = ExistReserva.Message;
                    return result;
                }

                if (ExistReserva.Data.Servicios.Any(s => s.Nombre == ExistServicio.Data.Nombre))
                {
                    result.Message = "El servicio ya está agregado a la reserva.";
                    return result;
                }


                ExistReserva.Data.Servicios.Add(ExistServicio.Data);

                ExistReserva.Data.CostoTotal += ExistServicio.Data.Precio != null ? ExistServicio.Data.Precio : 0;

                var opResult = await _reservarepository.UpdateAsync(ExistReserva.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                ReservaDto reservaDto = new ReservaDto()
                {
                    Id = opResult.Data.Id,
                    FechaInicio = opResult.Data.FechaInicio,
                    FechaFin = opResult.Data.FechaFin,
                    CostoTotal = opResult.Data.CostoTotal,
                    Estado = opResult.Data.Estado
                };                

                result.Success = true;
                result.Message = "Servicio agregado correctamente.";
                result.Data = reservaDto;

            }
            catch (Exception ex)
            {
                result.Message = $"Error al añadir el servicio adicional a la reserva.";
            }
            return result;
        }
        public async Task<ServiceResult> RemoveServicioAdicional(int id, string nombreServicio)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ExistServicio = await _servicioAdicionalRepository.GetByNombreAsync(nombreServicio);
                if (!ExistServicio.Success)
                {
                    result.Message = ExistServicio.Message;
                    return result;
                }
                var ExistReserva = await _reservarepository.GetByIdAsync(id);
                if (!ExistReserva.Success)
                {
                    result.Message = ExistReserva.Message;
                    return result;
                }

                var servicioEnReserva = ExistReserva.Data.Servicios
                                        .FirstOrDefault(s => s.Nombre == ExistServicio.Data.Nombre);

                if (servicioEnReserva == null)
                {
                    result.Message = "El servicio no está agregado en esta reserva.";
                    return result;
                }

                ExistReserva.Data.Servicios.Remove(servicioEnReserva);


                ExistReserva.Data.Servicios.Remove(ExistServicio.Data);
                ExistReserva.Data.CostoTotal = Math.Max(0, ExistReserva.Data.CostoTotal - (ExistServicio.Data.Precio != null ? ExistServicio.Data.Precio : 0));

                var opResult = await _reservarepository.UpdateAsync(ExistReserva.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }
                ReservaDto reservaDto = new ReservaDto()
                {
                    Id = opResult.Data.Id,
                    FechaInicio = opResult.Data.FechaInicio,
                    FechaFin = opResult.Data.FechaFin,
                    CostoTotal = opResult.Data.CostoTotal,
                    Estado = opResult.Data.Estado
                };
                result.Success = true;
                result.Message = "Servicio removido correctamente.";
                result.Data = reservaDto;
            }
            catch (Exception ex)
            {
                result.Message = $"Error al remover el servicio adicional a la reserva.";
            }
            return result;
        }
        public async Task<ServiceResult> GetServiciosByReservaId(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var reserva = await _reservarepository.GetByIdAsync(id);
                if (!reserva.Success)
                {
                    result.Message = reserva.Message;
                    return result;
                }

                var servicios = reserva.Data.Servicios ?? new List<ServicioAdicional>();


                var servicioDtos = servicios.Select(s => new ServicioAdicionalDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    Precio = s.Precio,
                    Estado = s.Estado
                }).ToList();

                result.Success = true;
                result.Data = servicioDtos;
                result.Message = "Se obtuvieron los servicios adicionales correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener los servicios adicionales de la reserva: {ex.Message}";
            }
            return result;
        }
    }
}
