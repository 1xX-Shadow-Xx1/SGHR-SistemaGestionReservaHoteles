
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;

namespace SGHR.Application.ValidatorServices.Usuarios
{
    public class UsuarioValidatorServices
    {
        private readonly BaseValidatorServices baseValidatorServices;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioValidatorServices(IUsuarioRepository usuarioRepository)
        {
            baseValidatorServices = new BaseValidatorServices();
            _usuarioRepository = usuarioRepository;
        }

        public bool ValidateSave(CreateUsuarioDto? usuarioDto, out string errorMessage)
        {
            if (!baseValidatorServices.IsNull<CreateUsuarioDto>(usuarioDto, "El usuario", out errorMessage)) return false;
            var val = baseValidatorServices.ExistePorCampoAsync<Usuario>(u => u.Correo == usuarioDto.Correo, _usuarioRepository, "un usuario", "ese correo");
            if (val.Result.Existe)
            {
                errorMessage = val.Result.ErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        } 

        public bool ValidateDelete(int id, out string errorMessage)
        {
            if(!baseValidatorServices.IdValidate(id, out errorMessage)) return false;
            var val = baseValidatorServices.ExistePorCampoAsync<Usuario>(u => u.Id == id, _usuarioRepository, "un usuario", "ese id"); 
            if (val.Result.Existe)
            {
                errorMessage = val.Result.ErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
