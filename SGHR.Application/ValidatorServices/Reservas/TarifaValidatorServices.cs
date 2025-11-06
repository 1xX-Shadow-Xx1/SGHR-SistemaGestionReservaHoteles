
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Entities.Configuration.Reservas;
using SGHR.Persistence.Interfaces.Habitaciones;
using SGHR.Persistence.Interfaces.Reservas;

namespace SGHR.Application.ValidatorServices.Reservas
{
    public class TarifaValidatorServices
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly BaseValidatorServices baseValidatorServices;

        public TarifaValidatorServices(ITarifaRepository tarifaRepository, ICategoriaRepository categoriaRepository) 
        {
            _tarifaRepository = tarifaRepository;
            _categoriaRepository = categoriaRepository;
            baseValidatorServices = new BaseValidatorServices();
        }

        public bool ValidateCreate(CreateTarifaDto? CreateDto, out string errorMessage)
        {
            if(!baseValidatorServices.IsNull<CreateTarifaDto>(CreateDto, "la tarifa", out errorMessage)) return false;
            var vali = baseValidatorServices.BuscarExistCampoAsync<Categoria>(c => c.Nombre == CreateDto.NombreCategoria, _categoriaRepository, "Categoria");
            if (!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            var vali3 = baseValidatorServices.ExistePorCampoAsync<Tarifa>(t => t.IdCategoria == vali.Result.data.Id && t.Temporada == CreateDto.Temporada, _tarifaRepository, "una tarifa", "esa categoria en la misma temporada");
            if(vali3.Result.Existe)
            {
                errorMessage = vali3.Result.ErrorMessage;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        public bool ValidateDelete(int id, out string errorMessage)
        {
            if(!baseValidatorServices.IdValidate(id, out errorMessage)) return false;
            var vali = baseValidatorServices.ExistePorIdAsync<Tarifa>(id, _tarifaRepository);
            if (!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        public bool ValidateGetById(int id, out string errorMessage)
        {
            if(!baseValidatorServices.IdValidate(id, out errorMessage)) return false;
            errorMessage = string.Empty;
            return true;
        }

        public bool ValidateUpdate(UpdateTarifaDto? UpdateDto, out string errorMessage)
        {
            if (!baseValidatorServices.IdValidate(UpdateDto.Id, out errorMessage)) return false;
            if (!baseValidatorServices.IsNull<UpdateTarifaDto>(UpdateDto, "la tarifa", out errorMessage)) return false;
            var vali = baseValidatorServices.BuscarExistCampoAsync<Categoria>(c => c.Nombre == UpdateDto.NombreCategoria, _categoriaRepository, "Categoria");
            if (!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            var vali3 = baseValidatorServices.ExistePorCampoAsync<Tarifa>(t => t.IdCategoria == vali.Result.data.Id && t.Temporada == UpdateDto.Temporada && t.Id != UpdateDto.Id, _tarifaRepository, "una tarifa", "esa categoria en la misma temporada");
            if (vali3.Result.Existe)
            {
                errorMessage = vali3.Result.ErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
