
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.Services.Reservas
{
    public class TarifaServices : ITarifaServices
    {
        private readonly ILogger<TarifaServices> _logger;
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public TarifaServices(ILogger<TarifaServices> logger,
                              ITarifaRepository tarifaRepository,
                              ICategoriaRepository categoriaRepository)
        {
            _logger = logger;
            _tarifaRepository = tarifaRepository;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateTarifaDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if(CreateDto == null)
            {
                result.Message = "La tarifa no puede ser nula.";
                return result;
            }
            try
            {
                var Tarifas = await _tarifaRepository.GetAllAsync();
                if (!Tarifas.Success)
                {
                    result.Message = Tarifas.Message;
                    return result;
                }
                var Categorias = await _categoriaRepository.GetAllAsync();
                if (!Categorias.Success)
                {
                    result.Message = Categorias.Message;
                    return result;
                }

                var ExistCategoria = Categorias.Data.FirstOrDefault(u => u.Nombre == CreateDto.NombreCategoria);
                if(ExistCategoria == null)
                {
                    result.Message = $"No se encontro la categoria, introduce el nombre de una categoria ya registrada.";
                    return result;
                }



                var ExistTarifa = Tarifas.Data.FirstOrDefault(u => u.Temporada == CreateDto.Temporada && u.IdCategoria == ExistCategoria.Id);
                if (ExistTarifa != null)
                {
                    result.Message = $"Ya existe una tarifa para esa temporada con la misma categoria.";
                    return result;
                }

                Tarifa tarifa = new Tarifa()
                {
                    IdCategoria = ExistCategoria.Id,
                    Temporada = CreateDto.Temporada,
                    Precio = CreateDto.Precio
                };

                var opResult = await _tarifaRepository.SaveAsync(tarifa);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                TarifaDto tarifaDto = new TarifaDto()
                {
                    Id = opResult.Data.Id,
                    Temporada = opResult.Data.Temporada,
                    NombreCategoria = ExistCategoria.Nombre,
                    Precio = opResult.Data.Precio
                };

                result.Success = true;
                result.Data = tarifaDto;
                result.Message = $"Se a registrado la tarifa correctamente.";

            }
            catch(Exception ex)
            {
                result.Message = $"Error al registrar la tarifa: {ex.Message}";
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
                var existTarifa = await _tarifaRepository.GetByIdAsync(id);
                if (!existTarifa.Success)
                {
                    result.Message = existTarifa.Message;
                    return result;
                }

                var OpResult = await _tarifaRepository.DeleteAsync(existTarifa.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Tarifa con id {existTarifa.Data.Id} eliminada correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar la tarifa: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListaTarifas = await _tarifaRepository.GetAllAsync();
                if (!ListaTarifas.Success)
                {
                    result.Message = ListaTarifas.Message;
                    return result;
                }
                var ListaCategorias = await _categoriaRepository.GetAllAsync();
                if (!ListaCategorias.Success)
                {
                    result.Message = ListaCategorias.Message;
                    return result;
                }

                var tarifaDtos = (
                    from c in ListaTarifas.Data
                    join u in ListaCategorias.Data on c.IdCategoria equals u.Id into userGroup
                    from u in userGroup.DefaultIfEmpty()
                    select new TarifaDto
                    {
                        Id = c.Id,
                        NombreCategoria = u.Nombre,
                        Temporada = c.Temporada,
                        Precio = c.Precio
                    }
                ).ToList();

                result.Success = true;
                result.Data = tarifaDtos;
                result.Message = "Se obtuvieron las tarifas correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Erro al obtener las tarifas.";
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
                var opResult = await _tarifaRepository.GetByIdAsync(id);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }
                
                var categoria = await _categoriaRepository.GetByIdAsync(opResult.Data.IdCategoria);
                if (!categoria.Success)
                {
                    result.Message = categoria.Message;
                    return result;
                }

                TarifaDto tarifaDto = new TarifaDto()
                {
                    Id = opResult.Data.Id,
                    Temporada = opResult.Data.Temporada,
                    NombreCategoria = categoria.Data.Nombre,
                    Precio = opResult.Data.Precio
                };

                result.Success = true;
                result.Data = tarifaDto;
                result.Message = $"Se obtuvo la tarifa con id {id} correctamnete.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener la tarifa: {ex.Message}";
            }
            return result;
        }
        public async Task<ServiceResult> UpdateAsync(UpdateTarifaDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "La tarifa no puede ser nula.";
                return result;
            }
            if (UpdateDto.Id <= 0)
            {
                result.Message = "El id es invalido.";
                return result;
            }
            try
            {
                var Tarifas = await _tarifaRepository.GetAllAsync();
                if (!Tarifas.Success)
                {
                    result.Message = Tarifas.Message;
                    return result;
                }
                var Categorias = await _categoriaRepository.GetAllAsync();
                if (!Categorias.Success)
                {
                    result.Message = Categorias.Message;
                    return result;
                }

                var tarifa = Tarifas.Data.FirstOrDefault(u => u.Id == UpdateDto.Id);
                if (tarifa == null)
                {
                    result.Message = "No se a encontrado la tarifa con ese id.";
                    return result;
                }

                var ExistCategoria = Categorias.Data.FirstOrDefault(u => u.Nombre == UpdateDto.NombreCategoria);
                if (ExistCategoria == null)
                {
                    result.Message = $"No se encontro la categoria, introduce el nombre de una categoria ya registrada.";
                    return result;
                }

                var ExistTarifa = Tarifas.Data.FirstOrDefault(u => u.Temporada == UpdateDto.Temporada && u.IdCategoria == ExistCategoria.Id && u.Id != UpdateDto.Id);
                if (ExistTarifa != null)
                {
                    result.Message = $"Ya existe un tarifas para esas temporada con la misma categoria.";
                    return result;
                }

                tarifa.IdCategoria = ExistCategoria.Id;
                tarifa.Temporada = UpdateDto.Temporada;
                tarifa.Precio = (decimal)UpdateDto.Precio;

                var opResult = await _tarifaRepository.UpdateAsync(tarifa);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                TarifaDto tarifaDto = new TarifaDto()
                {
                    Id = opResult.Data.Id,
                    Temporada = opResult.Data.Temporada,
                    NombreCategoria = ExistCategoria.Nombre,
                    Precio = opResult.Data.Precio
                };

                result.Success = true;
                result.Data = tarifaDto;
                result.Message = $"Se a actualizado la tarifa correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al actualiza la tarifa: {ex.Message}";
            }
            return result;
        }
    }
}
