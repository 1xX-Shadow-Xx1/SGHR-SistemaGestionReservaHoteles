
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;

namespace SGHR.Application.Interfaces.Habitaciones
{
    public interface ICategoriaService : IBaseServices<CreateCategoriaDto, UpdateCategoriaDto, DeleteCategoriaDto>
    {
    }
}
