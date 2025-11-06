
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Domain.Entities.Configuration.Usuers;
using SGHR.Domain.Repository;
using SGHR.Persistence.Interfaces.Users;

namespace SGHR.Application.ValidatorServices.Usuarios
{
    public class ClienteValidatorServices
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly BaseValidatorServices baseValidatorServices;
        private readonly IUsuarioRepository _usuarioRepository;
        public ClienteValidatorServices(IClienteRepository clienteRepository,
                                        IUsuarioRepository usuarioRepository)
        {
            _clienteRepository = clienteRepository;
            baseValidatorServices = new BaseValidatorServices();
            _usuarioRepository = usuarioRepository;
        }

        public bool ValidateCreate(CreateClienteDto? CreateDto, out string errorMessage)
        {
            if(!baseValidatorServices.IsNull<CreateClienteDto>(CreateDto, "El cliente", out errorMessage)) return false;
            var vali = baseValidatorServices.ExistePorCampoAsync<Cliente>(c => c.Cedula == CreateDto.Cedula, _clienteRepository, "un cliente", "esa cedula");
            if(!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            if (!string.IsNullOrEmpty(CreateDto.Correo))
            {
                var vali2 = baseValidatorServices.BuscarExistCampoAsync<Usuario>(c => c.Correo == CreateDto.Correo, _usuarioRepository, "Usuario");
                if (!vali2.Result.Existe)
                {
                    errorMessage = vali2.Result.ErrorMessage;
                    return false;
                }
                var vali3 = baseValidatorServices.ExistePorCampoAsync<Cliente>(c => c.IdUsuario == vali2.Result.data.Id, _clienteRepository, "un cliente", "ese correo");
                if (!vali3.Result.Existe)
                {
                    errorMessage = vali3.Result.ErrorMessage;
                    return false;
                }

            }          
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateDelete(int id, out string errorMessage)
        {
            if(!baseValidatorServices.IdValidate(id, out errorMessage)) return false;
            var vali = baseValidatorServices.ExistePorIdAsync(id, _clienteRepository);
            if (!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateGetByCedula(string campo, out string errorMessage)
        {
            if(!baseValidatorServices.stringNull(campo, "una cedula", out errorMessage)) return false;
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateGetById(int id, out string errorMessage)
        {
            if (!baseValidatorServices.IdValidate(id, out errorMessage)) return false;
            errorMessage = string.Empty;
            return true;
        }
        public bool ValidateUpdate(UpdateClienteDto? UpdateDto, out string errorMessage)
        {
            if(!baseValidatorServices.IsNull<UpdateClienteDto>(UpdateDto,"El cliente", out errorMessage)) return false;
            if (!baseValidatorServices.IdValidate(UpdateDto.Id, out errorMessage)) return false;
            var vali = baseValidatorServices.ExistePorIdAsync<Cliente>(UpdateDto.Id, _clienteRepository);
            var vali2 = baseValidatorServices.ExistePorCampoAsync<Cliente>(c => c.Cedula == UpdateDto.Cedula && c.Id != UpdateDto.Id, _clienteRepository, "un cliente", "esa cedula");
            if (!vali.Result.Existe)
            {
                errorMessage = vali.Result.ErrorMessage;
                return false;
            }
            if (!string.IsNullOrEmpty(UpdateDto.Correo))
            {
                var vali3 = baseValidatorServices.BuscarExistCampoAsync<Usuario>(c => c.Correo == UpdateDto.Correo, _usuarioRepository, "Usuario");
                if (!vali3.Result.Existe)
                {
                    errorMessage = vali3.Result.ErrorMessage;
                    return false;
                }

                var vali4 = baseValidatorServices.ExistePorCampoAsync<Cliente>(c => c.IdUsuario == vali3.Result.data.Id && c.Id != UpdateDto.Id, _clienteRepository, "un cliente", "ese correo");
                if (!vali4.Result.Existe)
                {
                    errorMessage = vali4.Result.ErrorMessage;
                    return false;
                }

            }
            errorMessage = string.Empty;
            return true;
        }
    }
}
