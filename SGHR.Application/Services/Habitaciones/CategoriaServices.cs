using Microsoft.Extensions.Logging;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Persistence.Interfaces.Habitaciones;

namespace SGHR.Application.Services.Habitaciones
{
    public class CategoriaServices : ICategoriaServices
    {
        public readonly ILogger<CategoriaServices> _logger;
        public readonly ICategoriaRepository _categoriaRepository;

        public CategoriaServices(ILogger<CategoriaServices> logger, 
                                 ICategoriaRepository categoriaRepository)
        {
            _logger = logger;
            _categoriaRepository = categoriaRepository;
        }

        public async Task<ServiceResult> CreateAsync(CreateCategoriaDto CreateDto)
        {
            ServiceResult result = new ServiceResult();
            if (CreateDto == null)
            {
                result.Message = "La categoria no puede ser nula.";
                return result;
            }
            try
            {

                var LisCategorias = await _categoriaRepository.GetAllAsync();
                if (!LisCategorias.Success)
                {
                    result.Message = LisCategorias.Message;
                    return result;
                }

                var existCategoriaName = LisCategorias.Data.FirstOrDefault(c => c.Nombre == CreateDto.Nombre);
                if (existCategoriaName != null)
                {
                    result.Message = "Ya existe una categoria registrado con ese nombre.";
                    return result;
                }

                var existCategoriaDescripcion = LisCategorias.Data.FirstOrDefault(c => c.Descripcion == CreateDto.Descripcion);
                if (existCategoriaDescripcion != null)
                {
                    result.Message = "Ya existe una categoria con esa descripcion.";
                    return result;
                }

                var categoria = new Categoria
                {
                    Nombre = CreateDto.Nombre,
                    Descripcion = CreateDto.Descripcion
                };

                var opResult = await _categoriaRepository.SaveAsync(categoria);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                CategoriaDto categoriaDto = new CategoriaDto()
                {
                    Id = categoria.Id,
                    Nombre = opResult.Data.Nombre,
                    Descripcion = opResult.Data.Descripcion
                };

                result.Success = true;
                result.Message = "Categoria creado exitosamente.";
                result.Data = categoriaDto;

            }
            catch (Exception ex)
            {
                result.Message = $"Error al crear la categoria: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id ingresado no es valido.";
                return result;
            }
            try
            {
                var existCategoria = await _categoriaRepository.GetByIdAsync(id);
                if (!existCategoria.Success)
                {
                    result.Message = $"No existe una categoria con ese id.";
                    return result;
                }

                var OpResult = await _categoriaRepository.DeleteAsync(existCategoria.Data);
                if (!OpResult.Success)
                {
                    result.Message = OpResult.Message;
                    return result;
                }
                result.Success = true;
                result.Message = $"Categoria {existCategoria.Data.Nombre} eliminado correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al eliminar la categoria: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var ListCategoria = await _categoriaRepository.GetAllAsync();
                if(!ListCategoria.Success)
                {
                    result.Message = ListCategoria.Message;
                    return result;
                }

                var ListCateforiaDto = ListCategoria.Data.Select(c => new CategoriaDto()
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion
                })
                .ToList();

                result.Success = true;
                result.Data = ListCateforiaDto;
                result.Message = "Se obtuvieron las categorias correctamente.";

            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener las categorias: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            if (id <= 0)
            {
                result.Message = "El id ingresado no es valido.";
                return result;
            }
            try
            {
                var Categoria = await _categoriaRepository.GetByIdAsync(id);
                if (!Categoria.Success)
                {
                    result.Message = Categoria.Message;
                    return result;
                }

                CategoriaDto CategoriaDto = new CategoriaDto()
                {
                    Id = Categoria.Data.Id,
                    Nombre = Categoria.Data.Nombre,
                    Descripcion = Categoria.Data.Descripcion
                };

                result.Success = true;
                result.Data = CategoriaDto;
                result.Message = "Se obtuvo la categoria correctamente.";
            }
            catch (Exception ex)
            {
                result.Message = $"Error al obtener la categoria por id: {ex.Message}";
            }
            return result;
        }

        public async Task<ServiceResult> UpdateAsync(UpdateCategoriaDto UpdateDto)
        {
            ServiceResult result = new ServiceResult();
            if (UpdateDto == null)
            {
                result.Message = "El cliente no puede ser nulo.";
                return result;
            }
            if(UpdateDto.Id <= 0)
            {
                result.Message = "El id ingresado no es valido.";
                return result;
            }
            try
            {
                var LisCategorias = await _categoriaRepository.GetAllAsync();
                if (!LisCategorias.Success)
                {
                    result.Message = LisCategorias.Message;
                    return result;
                }

                var existCategoriaName = LisCategorias.Data.FirstOrDefault(c => c.Nombre == UpdateDto.Nombre && c.Id != UpdateDto.Id);
                if (existCategoriaName != null)
                {
                    result.Message = "Ya existe una categoria registrado con ese nombre.";
                    return result;
                }

                var existCategoriaDescripcion = LisCategorias.Data.FirstOrDefault(c => c.Descripcion == UpdateDto.Descripcion && c.Id != UpdateDto.Id);
                if (existCategoriaDescripcion != null)
                {
                    result.Message = "Ya existe una categoria con esa descripcion.";
                    return result;
                }

                var Categoria = LisCategorias.Data.FirstOrDefault(c => c.Id == UpdateDto.Id);
                if (Categoria == null)
                {
                    result.Message = $"No se encontro la categoria con id {UpdateDto.Id}.";
                    return result;
                }

                Categoria.Nombre = UpdateDto.Nombre;
                Categoria.Descripcion = UpdateDto.Descripcion;

                var opResult = await _categoriaRepository.UpdateAsync(Categoria);
                if (!opResult.Success)
                {
                    result.Message = opResult.Message;
                    return result;
                }

                CategoriaDto categoriaDto = new CategoriaDto()
                {
                    Id = opResult.Data.Id,
                    Nombre = opResult.Data.Nombre,
                    Descripcion = opResult.Data.Descripcion
                };

                result.Success = true;
                result.Message = "Categoria actualizada exitosamente.";
                result.Data = categoriaDto;

            }
            catch (Exception ex)
            {
                result.Message = $"Error al actualizar la categoria: {ex.Message}";
            }
            return result;
        }
    }
}
