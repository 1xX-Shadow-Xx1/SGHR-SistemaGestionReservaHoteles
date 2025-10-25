using SGHR.Domain.Entities.Configuration.Operaciones;
using SGHR.Domain.Enum.Operaciones;

namespace SGHR.Domain.Validators.ConfigurationRules.Operaciones
{
    public class MantenimientoValidator
    {
        public bool Validate(Mantenimiento mantenimiento, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(mantenimiento, "Mantenimiento", out errorMessage)) return false;

            // FK Piso (opcional)
            if (mantenimiento.IdPiso.HasValue && mantenimiento.IdPiso <= 0)
            {
                errorMessage = "IdPiso inválido.";
                return false;
            }

            // FK Habitación
            if (!ValidationHelper.GreaterThanZero(mantenimiento.IdHabitacion, "IdHabitacion", out errorMessage)) return false;

            // Descripción (opcional)
            if (!string.IsNullOrEmpty(mantenimiento.Descripcion))
            {
                if (!ValidationHelper.MaxLength(mantenimiento.Descripcion, 1000, "Descripción", out errorMessage)) return false;
            }

            // Estado
            var estadosValidos = new[] { EstadoMantenimiento.Completado, EstadoMantenimiento.Pendiente, EstadoMantenimiento.Cancelado, EstadoMantenimiento.EnProceso }; // 0=pendiente, 1=en proceso, 2=finalizado
            if (!ValidationHelper.InList(mantenimiento.Estado, estadosValidos, "Estado", out errorMessage)) return false;

            // Fecha inicio
            if (mantenimiento.FechaInicio == DateTime.MinValue)
            {
                errorMessage = "Fecha de inicio es obligatoria.";
                return false;
            }

            // Fecha fin (opcional)
            if (mantenimiento.FechaFin.HasValue && mantenimiento.FechaFin < mantenimiento.FechaInicio)
            {
                errorMessage = "Fecha de fin no puede ser anterior a la fecha de inicio.";
                return false;
            }

            // FK Usuario que realizó
            if (!ValidationHelper.GreaterThanZero(mantenimiento.RealizadoPor, "RealizadoPor", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
