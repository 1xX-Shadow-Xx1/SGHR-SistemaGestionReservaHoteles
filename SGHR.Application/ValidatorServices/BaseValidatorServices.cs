using SGHR.Domain.Repository;
using System.Diagnostics.CodeAnalysis;

namespace SGHR.Application.ValidatorServices
{
    public class BaseValidatorServices
    {
        // Método para verificar si un id es valido
        public bool IdValidate(int id, out string errorMessage)
        {
            if (id <= 0)
            {
                errorMessage = "El id ingresado no es valido.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        // Método para verificar si un string es null
        public bool stringNull(string texto, string fieldname, out string errorMessage)
        {
            if (string.IsNullOrEmpty(texto))
            {
                errorMessage = $"Tiene que ingresar {fieldname}.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        // Método para verificar si un DTO es null
        public bool IsNull<T>(T dto, string fielname, out string errorMessage)
            where T : class
        {
            if (dto == null)
            {
                errorMessage = $"{fielname} no puede ser nulo.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        // Valida si un Id existe en el repositorio
        public async Task<(bool Existe, string ErrorMessage)> ExistePorIdAsync<T>(
            int id,
            IBaseRepository<T> repository) where T : class
        {
            if (id <= 0)
                return (false, $"El id ingresado no es válido.");

            var entity = await repository.GetByIdAsync(id);
            if (!entity.Success)
                return (false, entity.Message);

            return (true, string.Empty);
        }

        // Valida si un campo existe en el repositorio
        public async Task<(bool Existe, string ErrorMessage)> ExistePorCampoAsync<T>(
        Func<T, bool> predicate,
        IBaseRepository<T> repository,
        string fieldname,
        string fieldvalor) where T : class
        {
            var entities = await repository.GetAllAsync();
            if (!entities.Success)
                return (false, entities.Message);

            var existe = entities.Data.Any(predicate);
            if (existe)
                return (false, $"Ya existe {fieldname} registrado con {fieldvalor}.");

            return (true, string.Empty);
        }

        // Valida si un campo existe en el repositorio
        public async Task<(bool Existe, string ErrorMessage, T? data)> BuscarExistCampoAsync<T>(
        Func<T, bool> predicate,
        IBaseRepository<T> repository,
        string fieldname) where T : class
        {
            var entities = await repository.GetAllAsync();
            if (!entities.Success)
                return (false, entities.Message, null);

            var existe = entities.Data.Any(predicate);
            if (!existe)
                return (false, $"{fieldname} no encontrado.", null);

            return (true, string.Empty, entities.Data.First());
        }

    }
}
