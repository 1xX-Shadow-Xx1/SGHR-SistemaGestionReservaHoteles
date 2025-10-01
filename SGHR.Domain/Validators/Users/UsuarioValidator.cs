using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Usuers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Validators.Users
{
    public static class UsuarioValidator
    {
        public static OperationResult<Usuario> Validate(Usuario entity)
        {
            if (entity == null)
                return OperationResult<Usuario>.Fail("El usuario no puede ser nulo");

            if (string.IsNullOrWhiteSpace(entity.Nombre))
                return OperationResult<Usuario>.Fail("El nombre no puede estar vacío");

            if (entity.Nombre.Length > 100)
                return OperationResult<Usuario>.Fail("El nombre no puede tener más de 100 caracteres");

            if (string.IsNullOrWhiteSpace(entity.Correo))
                return OperationResult<Usuario>.Fail("El correo no puede estar vacío");

            if (string.IsNullOrWhiteSpace(entity.Contrasena))
                return OperationResult<Usuario>.Fail("La contraseña no puede estar vacía");

            if (entity.Contrasena.Length > 255)
                return OperationResult<Usuario>.Fail("La contraseña no puede tener más de 255 caracteres");

            return OperationResult<Usuario>.Ok(entity);
        }
    }
}
