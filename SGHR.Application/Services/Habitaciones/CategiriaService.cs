using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Categorias
{
    public class CategiriaService : ICategoriaService
    {
        private readonly ILogger<CategiriaService> _logger;
        public readonly ICategoriaRepository _categoriaRepository;

        public CategiriaService(ICategoriaRepository categoriaRepository,
                              ILogger<CategiriaService> logger)
        {
            _categoriaRepository = categoriaRepository;
            _logger = logger;

        }

        public async Task<ServiceResult> GetAll()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las categorias.");

            try
            {
                var opResult = await _categoriaRepository.GetAll();
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categorias obtenidas correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener las categorias.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las categorias.");
                result.Success = false;
                result.Message = "Error al obtener las categorias.";
            }

            return result;
        }

        public async Task<ServiceResult> GetById(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de categoria por ID: {Id}", id);

            try
            {
                var opResult = await _categoriaRepository.GetById(id);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al obtener la categoria.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la categoria.");
                result.Success = false;
                result.Message = "Error al obtener la categoria.";
            }
            return result;
        }

        public async Task<ServiceResult> Update(UpdateCategoriaDto updateCategoriaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de categoria.", updateCategoriaDto);

            try
            {
                if (updateCategoriaDto == null)
                {
                    result.Success = false;
                    result.Message = "Error: la categoria no puede ser nula";
                    return result;
                }
                var existingCategoriaResult = await _categoriaRepository.GetById(updateCategoriaDto.Id);
                if (!existingCategoriaResult.Success || existingCategoriaResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: la categoria no existe";
                    return result;
                }

                var existingCategoria = existingCategoriaResult.Data;
                existingCategoria.Nombre = updateCategoriaDto.Nombre;
                existingCategoria.Descripcion = updateCategoriaDto.Descripcion;

                var opResult = await _categoriaRepository.Update(existingCategoria);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria actualizada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al actualizar la categoria.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la categoria.");
                result.Success = false;
                result.Message = "Error al actualizar la categoria.";
            }
            return result;
        }

        public async Task<ServiceResult> Remove(DeleteCategoriaDto deleteCategoriaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de categoria: {Dto}", deleteCategoriaDto);

            try
            {
                if (deleteCategoriaDto == null || deleteCategoriaDto.Id <= 0)
                {
                    result.Success = false;
                    result.Message = "Error: la categoria no puede ser nula y debe tener un ID válido.";
                    return result;
                }
                var existingCategoriaResult = await _categoriaRepository.GetById(deleteCategoriaDto.Id);
                if (!existingCategoriaResult.Success || existingCategoriaResult.Data == null)
                {
                    result.Success = false;
                    result.Message = "Error: la categoria a eliminar no existe.";
                    return result;
                }

                var opResult = await _categoriaRepository.Delete(existingCategoriaResult.Data);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria eliminada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al eliminar la categoria.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la categoria.");
                result.Success = false;
                result.Message = "Error al eliminar la categoria.";
            }
            return result;
        }

        public async Task<ServiceResult> Save(CreateCategoriaDto createCategoriaDto)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando creación de categoria.", createCategoriaDto);

            try
            {
                if (createCategoriaDto == null)
                {
                    result.Success = false;
                    result.Message = "Error: la categoria no puede ser nula";
                    return result;
                }


                Categoria categoria = new Categoria
                {
                    Nombre = createCategoriaDto.Nombre,
                    Descripcion = createCategoriaDto.Descripcion
                };


                var opResult = await _categoriaRepository.Save(categoria);
                if (opResult.Success)
                {
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria creada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Error al crear la categoria.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la categoria.");
                result.Success = false;
                result.Message = "Error al crear la categoria.";
            }
            return result;
        }
    }
}
