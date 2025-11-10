
using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.ValidatorServices.Reservas;
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
            try
            {
                var validate = new TarifaValidatorServices(_tarifaRepository, _categoriaRepository).ValidateCreate(CreateDto, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }

                var Categoria = await _categoriaRepository.GetByNombreAsync(CreateDto.NombreCategoria);
                if (!Categoria.Success)
                {
                    result.Message = Categoria.Message;
                    return result;
                }

                Tarifa tarifa = new Tarifa()
                {
                    IdCategoria = Categoria.Data.Id,
                    Fecha_inicio = CreateDto.Fecha_inicio,
                    Fecha_fin = CreateDto.Fecha_fin,
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
                    Fecha_inicio = opResult.Data.Fecha_inicio,
                    Fecha_fin = opResult.Data.Fecha_fin,
                    NombreCategoria = Categoria.Data.Nombre,
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
            try
            {
                var validate = new TarifaValidatorServices(_tarifaRepository, _categoriaRepository).ValidateDelete(id, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var tarifa = await _tarifaRepository.GetByIdAsync(id);
                if (!tarifa.Success)
                {
                    result.Message = tarifa.Message;
                    return result;
                }

                var OpResult = await _tarifaRepository.DeleteAsync(tarifa.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Tarifa con id {tarifa.Data.Id} eliminada correctamente.";

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
                        Fecha_inicio = c.Fecha_inicio,
                        Fecha_fin = c.Fecha_fin,
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
            try
            {
                var validate = new TarifaValidatorServices(_tarifaRepository, _categoriaRepository).ValidateGetById(id, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
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
                    Fecha_inicio = opResult.Data.Fecha_inicio,
                    Fecha_fin = opResult.Data.Fecha_fin,
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
            try
            {
                var validate = new TarifaValidatorServices(_tarifaRepository, _categoriaRepository).ValidateUpdate(UpdateDto, out string errorMessage);
                if (!validate)
                {
                    result.Message = errorMessage;
                    return result;
                }
                var tarifa = await _tarifaRepository.GetByIdAsync(UpdateDto.Id);
                if (!tarifa.Success)
                {
                    result.Message = tarifa.Message;
                    return result;
                }

                var categoria = await _categoriaRepository.GetByNombreAsync(UpdateDto.NombreCategoria);
                if (categoria == null)
                {
                    result.Message = categoria.Message;
                    return result;
                }

                tarifa.Data.IdCategoria = categoria.Data.Id;
                tarifa.Data.Fecha_inicio = UpdateDto.Fecha_inicio;
                tarifa.Data.Fecha_fin = UpdateDto.Fecha_fin;
                tarifa.Data.Precio = (decimal)UpdateDto.Precio;

                var opResult = await _tarifaRepository.UpdateAsync(tarifa.Data);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                TarifaDto tarifaDto = new TarifaDto()
                {
                    Id = opResult.Data.Id,
                    Fecha_inicio = opResult.Data.Fecha_inicio,
                    Fecha_fin = opResult.Data.Fecha_fin,
                    NombreCategoria = categoria.Data.Nombre,
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
