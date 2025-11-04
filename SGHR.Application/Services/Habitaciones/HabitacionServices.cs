
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitacion;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Habitaciones
{
    public class HabitacionServices : IHabitacionServices
    {
        private readonly ILogger<HabitacionServices> _logger;
        private readonly IHabitacionRepository _habitacionRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IPisoRepository _pisoRepository;
        private readonly IAmenityRepository _amenityRepository;

        public HabitacionServices(ILogger<HabitacionServices> logger, 
                                  IHabitacionRepository habitacionRepository,
                                  ICategoriaRepository categoriaRepository,
                                  IPisoRepository pisoRepository,
                                  IAmenityRepository amenityRepository)
        {
            _logger = logger;
            _habitacionRepository = habitacionRepository;
            _categoriaRepository = categoriaRepository;
            _pisoRepository = pisoRepository;
            _amenityRepository = amenityRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateHabitacionDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "La habitacion no puede ser nula.";
                return result;
            }
            try
            {
                var ListHabitacion = await _habitacionRepository.GetAllAsync();
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }

                var ExistHabitacion = ListHabitacion.Data.FirstOrDefault(h => h.Numero == CreateDto.Numero);
                if (ExistHabitacion != null)
                {
                    result.Message = "Ya existe una habitacion con ese numero.";
                    return result;
                }

                var ListCategory = await _categoriaRepository.GetAllAsync();
                if (!ListCategory.Success)
                {
                    result.Message = ListCategory.Message;
                    return result;
                }

                var Category = ListCategory.Data.FirstOrDefault(c => c.Nombre == CreateDto.CategoriaName);
                if(Category == null)
                {
                    result.Message = $"No existe una categoria con ese nombre.";
                    return result;
                }

                var ListPiso = await _pisoRepository.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }

                var Piso = ListPiso.Data.FirstOrDefault(p => p.NumeroPiso == CreateDto.NumeroPiso);
                if(Piso == null)
                {
                    result.Message = "No existe un piso con ese numero.";
                    return result;
                }

                var ListAmenity = await _amenityRepository.GetAllAsync();
                if (!ListAmenity.Success)
                {
                    result.Message = ListAmenity.Message;
                }
                Habitacion habitacion = new Habitacion()
                {
                    IdCategoria = Category.Id,
                    IdPiso = Piso.Id,
                    Capacidad = CreateDto.Capacidad,
                    Numero = CreateDto.Numero
                };

                if (!string.IsNullOrWhiteSpace(CreateDto.AmenityName))
                {
                    var Amenity = ListAmenity.Data.FirstOrDefault(a => a.Nombre == CreateDto.AmenityName);
                    if (Amenity == null)
                    {
                        result.Message = "No existe un amenity con ese nombre.";
                        return result;
                    }

                    habitacion.IdAmenity = Amenity.Id;
                }

                var opResult = await _habitacionRepository.SaveAsync(habitacion);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                HabitacionDto habitacionDto = new HabitacionDto()
                {
                    Id = opResult.Data.Id,
                    CategoriaName = Category.Nombre,
                    numeroPiso = Piso.NumeroPiso,
                    AmenityName = CreateDto.AmenityName,
                    Numero = opResult.Data.Numero,
                    Capacidad = opResult.Data.Capacidad,
                    Estado = opResult.Data.Estado.ToString()
                };

                result.Success = true;
                result.Message = "Se a registrado la habitacion correctamente.";
                result.Data = habitacionDto;


            }
            catch (Exception ex)
            {
                result.Message = $"Error creado la habitacion: {ex.Message}";
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
                var existHabitacion = await _habitacionRepository.GetByIdAsync(id);
                if (!existHabitacion.Success)
                {
                    result.Message = $"No existe una habitacion con ese id.";
                    return result;
                }

                var OpResult = await _habitacionRepository.DeleteAsync(existHabitacion.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Habitacion numero {existHabitacion.Data.Numero} eliminada correctamente.";


            }
            catch (Exception ex)
            {
                result.Message = $"Error eliminando la habitacion: {ex.Message}";
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
                var ListCategory = await _categoriaRepository.GetAllAsync();
                if (!ListCategory.Success)
                {
                    result.Message = ListCategory.Message;
                    return result;
                }
                var ListPiso = await _pisoRepository.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }
                var ListAmenity = await _amenityRepository.GetAllAsync();
                if (!ListAmenity.Success)
                {
                    result.Message = ListAmenity.Message;
                }


                var HabitacionDtos = (
                    from h in ListHabitacion.Data
                    join c in ListCategory.Data on h.IdCategoria equals c.Id into catGroup
                    from cat in catGroup.DefaultIfEmpty()

                    join p in ListPiso.Data on h.IdPiso equals p.Id into pisoGroup
                    from piso in pisoGroup.DefaultIfEmpty()

                    join a in ListAmenity.Data on h.IdAmenity equals a.Id into amenGroup
                    from amen in amenGroup.DefaultIfEmpty()

                    select new HabitacionDto
                    {
                        Id = h.Id,
                        Numero = h.Numero,
                        Capacidad = h.Capacidad,
                        Estado = h.Estado.ToString(),
                        CategoriaName = cat != null ? cat.Nombre : "Sin categoría",
                        numeroPiso = piso != null ? piso.NumeroPiso : 0,
                        AmenityName = amen != null ? amen.Nombre : "Sin amenidad"
                    }
                ).ToList();


                result.Success = true;
                result.Data = HabitacionDtos;
                result.Message = "Se obtuvieron las habitaciones correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error obteniendo las habitaciones: {ex.Message}";
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
                var category = await _categoriaRepository.GetByIdAsync(Habitacion.Data.IdCategoria);
                if(!category.Success)
                {
                    result.Message = category.Message;
                    return result;
                }
                var piso = await _pisoRepository.GetByIdAsync(Habitacion.Data.IdPiso);
                if (!piso.Success)
                {
                    result.Message = piso.Message;
                    return result;
                }
                var amenity = await _amenityRepository.GetByIdAsync(Habitacion.Data.IdPiso);
                if (!amenity.Success)
                {
                    result.Message = amenity.Message;
                    return result;
                }

                HabitacionDto HabitacionDto = new HabitacionDto()
                {
                    Id = Habitacion.Data.Id,
                    CategoriaName = category.Data.Nombre,
                    AmenityName = category.Data.Nombre,
                    numeroPiso = piso.Data.NumeroPiso,
                    Numero = Habitacion.Data.Numero,
                    Capacidad = Habitacion.Data.Capacidad,
                    Estado = Habitacion.Data.Estado.ToString()
                };

                result.Success = true;
                result.Data = HabitacionDto;
                result.Message = "Se obtuvo la Habitacion correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener la habitacion por id: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetAllDisponiblesAsync()
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
                var ListCategory = await _categoriaRepository.GetAllAsync();
                if (!ListCategory.Success)
                {
                    result.Message = ListCategory.Message;
                    return result;
                }
                var ListPiso = await _pisoRepository.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }
                var ListAmenity = await _amenityRepository.GetAllAsync();
                if (!ListAmenity.Success)
                {
                    result.Message = ListAmenity.Message;
                }

                var Listdisponibles = ListHabitacion.Data.Where(h => h.Estado == EstadoHabitacion.Disponible).ToList();
                if(Listdisponibles.Count() == 0)
                {
                    result.Message = "No hay habitaciones disponibles.";
                    return result;
                }

                var HabitacionDtos = (
                    from h in Listdisponibles
                    join c in ListCategory.Data on h.IdCategoria equals c.Id into catGroup
                    from cat in catGroup.DefaultIfEmpty()

                    join p in ListPiso.Data on h.IdPiso equals p.Id into pisoGroup
                    from piso in pisoGroup.DefaultIfEmpty()

                    join a in ListAmenity.Data on h.IdAmenity equals a.Id into amenGroup
                    from amen in amenGroup.DefaultIfEmpty()

                    select new HabitacionDto
                    {
                        Id = h.Id,
                        Numero = h.Numero,
                        Capacidad = h.Capacidad,
                        Estado = h.Estado.ToString(),
                        CategoriaName = cat != null ? cat.Nombre : "Sin categoría",
                        numeroPiso = piso != null ? piso.NumeroPiso : 0,
                        AmenityName = amen != null ? amen.Nombre : "Sin amenidad"
                    }
                ).ToList();


                result.Success = true;
                result.Data = HabitacionDtos;
                result.Message = "Se obtuvieron las habitaciones disponibles correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener las habitaciones disponibles: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UpdateHabitacionDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if(UpdateDto == null)
            {
                result.Message = "La habitacion no puede ser nula.";
                return result;
            }
            try
            {
                var ListHabitacion = await _habitacionRepository.GetAllAsync();
                if (!ListHabitacion.Success)
                {
                    result.Message = ListHabitacion.Message;
                    return result;
                }

                var Habitacion = ListHabitacion.Data.FirstOrDefault(h => h.Id == UpdateDto.Id);
                if (Habitacion == null)
                {
                    result.Message = "No se encontro una habitacion con ese id.";
                }

                var ExistHabitacion = ListHabitacion.Data.FirstOrDefault(h => h.Numero == UpdateDto.Numero && h.Id != UpdateDto.Id);
                if (ExistHabitacion != null)
                {
                    result.Message = "Ya existe una habitacion con ese numero.";
                    return result;
                }

                var ListCategory = await _categoriaRepository.GetAllAsync();
                if (!ListCategory.Success)
                {
                    result.Message = ListCategory.Message;
                    return result;
                }

                var Category = ListCategory.Data.FirstOrDefault(c => c.Nombre == UpdateDto.CategoriaName);
                if (Category == null)
                {
                    result.Message = $"No existe una categoria con ese nombre.";
                    return result;
                }

                var ListPiso = await _pisoRepository.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }

                var Piso = ListPiso.Data.FirstOrDefault(p => p.NumeroPiso == UpdateDto.numeroPiso);
                if (Piso == null)
                {
                    result.Message = "No existe un piso con ese numero.";
                    return result;
                }

                var ExisAmenity = await _amenityRepository.GetAllAsync();
                if (!ExisAmenity.Success)
                {
                    result.Message = ExisAmenity.Message;
                }

                Habitacion.IdCategoria = Category.Id;
                Habitacion.IdPiso = Piso.Id;
                Habitacion.Numero = UpdateDto.Numero;
                Habitacion.Capacidad = UpdateDto.Capacidad;
                if (Enum.TryParse<EstadoHabitacion>(UpdateDto.Estado, out var estado))
                {
                    Habitacion.Estado = estado;
                }


                if (!string.IsNullOrWhiteSpace(UpdateDto.AmenityName))
                {
                    var Amenity = ExisAmenity.Data.FirstOrDefault(a => a.Nombre == UpdateDto.AmenityName);
                    if (Amenity == null)
                    {
                        result.Message = "No existe un amenity con ese nombre.";
                        return result;
                    }

                    Habitacion.IdAmenity = Amenity.Id;
                }

                var opResult = await _habitacionRepository.UpdateAsync(Habitacion);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                HabitacionDto habitacionDto = new HabitacionDto()
                {
                    Id = opResult.Data.Id,
                    CategoriaName = Category.Nombre,
                    numeroPiso = Piso.NumeroPiso,
                    AmenityName = UpdateDto.AmenityName,
                    Numero = opResult.Data.Numero,
                    Capacidad = opResult.Data.Capacidad,
                    Estado = opResult.Data.Estado.ToString()
                };

                result.Success = true;
                result.Message = "Se a actualizado la habitacion correctamente.";
                result.Data = habitacionDto;


            }
            catch (Exception ex)
            {
                result.Message = $"Error al actualizar la habitacion: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllDisponibleDateAsync(DateTime fechainicio, DateTime fechafin)
        {
            ServiceResult result = new ServiceResult();
            if (fechainicio == default || fechafin == default)
            {
                result.Message = "Los 2 campos de fecha son obligatorios.";
                return result;
            }

            if (fechainicio >= fechafin)
            {
                result.Message = "La fecha de inicio debe ser menor que la fecha final.";
                return result;
            }

            try
            {
                var disponibles = await _habitacionRepository.GetAvailableAsync(fechainicio,fechafin);
                if(!disponibles.Success)
                {
                    result.Message = disponibles.Message;
                    return result;
                }

                if(disponibles.Data.Count() == 0)
                {
                    result.Message = $"No hay habitaciones disponibles entre el {fechainicio} hata {fechafin}";
                }

                var ListCategory = await _categoriaRepository.GetAllAsync();
                if (!ListCategory.Success)
                {
                    result.Message = ListCategory.Message;
                    return result;
                }
                var ListPiso = await _pisoRepository.GetAllAsync();
                if (!ListPiso.Success)
                {
                    result.Message = ListPiso.Message;
                    return result;
                }
                var ListAmenity = await _amenityRepository.GetAllAsync();
                if (!ListAmenity.Success)
                {
                    result.Message = ListAmenity.Message;
                }

                var HabitacionDtos = (
                    from h in disponibles.Data
                    join c in ListCategory.Data on h.IdCategoria equals c.Id into catGroup
                    from cat in catGroup.DefaultIfEmpty()

                    join p in ListPiso.Data on h.IdPiso equals p.Id into pisoGroup
                    from piso in pisoGroup.DefaultIfEmpty()

                    join a in ListAmenity.Data on h.IdAmenity equals a.Id into amenGroup
                    from amen in amenGroup.DefaultIfEmpty()

                    select new HabitacionDto
                    {
                        Id = h.Id,
                        Numero = h.Numero,
                        Capacidad = h.Capacidad,
                        Estado = h.Estado.ToString(),
                        CategoriaName = cat != null ? cat.Nombre : "Sin categoría",
                        numeroPiso = piso != null ? piso.NumeroPiso : 0,
                        AmenityName = amen != null ? amen.Nombre : "Sin amenidad"
                    }
                ).ToList();


                result.Success = true;
                result.Data = HabitacionDtos;
                result.Message = $"Se obtuvieron las habitaciones disponibles en el rango de fechas correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener la habitaciones disponibles en el rango de fechas: {ex.Message}";
            }
            return result;
        }
    }
}
