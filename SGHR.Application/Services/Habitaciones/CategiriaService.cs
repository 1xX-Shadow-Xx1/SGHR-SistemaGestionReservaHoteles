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

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de todas las categorias.");

            try
            {
                var opResult = await _categoriaRepository.GetAllAsync();
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvieron {count} categorias correctamente.", opResult.Data.Count);
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
        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando obtención de categoria por ID: {Id}", id);

            try
            {
                var opResult = await _categoriaRepository.GetByIdAsync(id);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se obtuvo una categoria con Id {id} correctamente.", id);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria obtenida correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
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
        public async Task<ServiceResult> UpdateAsync(UpdateCategoriaDto updateCategoriaDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando actualización de categoria.", updateCategoriaDto);

            try
            {
                var existingCategoriaResult = await _categoriaRepository.GetByIdAsync(updateCategoriaDto.Id);
                if (!existingCategoriaResult.Success || existingCategoriaResult.Data == null)
                {
                    result.Success = false;
                    result.Message = existingCategoriaResult.Message;
                    return result;
                }

                var existingCategoria = existingCategoriaResult.Data;
                existingCategoria.Nombre = updateCategoriaDto.Nombre;
                existingCategoria.Descripcion = updateCategoriaDto.Descripcion;

                var opResult = await _categoriaRepository.UpdateAsync(existingCategoria, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a actualizado la categoria con Id {id} correctamente.", updateCategoriaDto.Id);
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
        public async Task<ServiceResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            _logger.LogInformation("Iniciando eliminación de categoria: {Id}", id);

            try
            {
                if (id < 0)
                {
                    result.Success = false;
                    result.Message = "La categoria debe tener un ID válido.";
                    return result;
                }
                var CatagoriaExist = await _categoriaRepository.GetByIdAsync(id);
                if (!CatagoriaExist.Success)
                {
                    result.Success = false;
                    result.Message = CatagoriaExist.Message;
                    return result;
                }

                var opResult = await _categoriaRepository.DeleteAsync(CatagoriaExist.Data, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a eliminado una categoria con Id {id} correctamente.", id);
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
        public async Task<ServiceResult> CreateAsync(CreateCategoriaDto createCategoriaDto, int? idsesion = null)
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


                var opResult = await _categoriaRepository.SaveAsync(categoria, idsesion);
                if (opResult.Success)
                {
                    _logger.LogInformation("Se a creado una nueva categoria con el nombre {name} correctamente.", categoria.Nombre);
                    result.Success = true;
                    result.Data = opResult.Data;
                    result.Message = "Categoria creada correctamente.";
                }
                else
                {
                    result.Success = false;
                    result.Message = opResult.Message;
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
