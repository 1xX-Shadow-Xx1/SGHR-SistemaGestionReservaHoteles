using SGHR.Domain.Entities.Configuration.Operaciones;

namespace SGHR.Domain.Validators.ConfigurationRules.Operaciones
{
    public class AuditoryValidator
    {
        public bool Validate(Auditory auditoria, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(auditoria, "Auditoria", out errorMessage)) return false;

            // FK Usuario
            if (!ValidationHelper.GreaterThanZero(auditoria.IdUsuario, "IdUsuario", out errorMessage)) return false;

            // FK Sesion (opcional)
            if (auditoria.IdSesion.HasValue && auditoria.IdSesion <= 0)
            {
                errorMessage = "IdSesion inválido.";
                return false;
            }

            // Operación
            if (!ValidationHelper.Required(auditoria.Operacion, "Operación", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(auditoria.Operacion, 20, "Operación", out errorMessage)) return false;

            // Detalle (opcional)
            if (!string.IsNullOrEmpty(auditoria.Detalle))
            {
                if (!ValidationHelper.MaxLength(auditoria.Detalle, 1000, "Detalle", out errorMessage)) return false;
            }

            // Fecha
            if (auditoria.Fecha == DateTime.MinValue)
            {
                errorMessage = "Fecha es obligatoria.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
