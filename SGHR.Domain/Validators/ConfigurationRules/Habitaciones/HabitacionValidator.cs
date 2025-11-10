using SGHR.Domain.Entities.Configuration.Habitaciones;
using SGHR.Domain.Enum.Habitaciones;



namespace SGHR.Domain.Validators.ConfigurationRules.Habitaciones
{
    public class HabitacionValidator
    {
        public bool Validate(Habitacion habitacion, out string errorMessage)
        {
            if (!ValidationHelper.NotNull(habitacion, "Habitación", out errorMessage)) return false;

            // FK Categoria
            if (!ValidationHelper.Required(habitacion.IdCategoria, "id de la categoria", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(habitacion.IdCategoria, "id de la categoria", out errorMessage)) return false;

            // FK Piso
            if (!ValidationHelper.Required(habitacion.IdPiso, "id del piso", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(habitacion.IdPiso, "id del piso", out errorMessage)) return false;

            // FK Amenity
            if (!ValidationHelper.Required(habitacion.IdAmenity, "id del amenity", out errorMessage)) return false;
            if (!ValidationHelper.GreaterThanZero(habitacion.IdAmenity, "id del amenity", out errorMessage)) return false;

            // Número
            if (!ValidationHelper.Required(habitacion.Numero, "Número", out errorMessage)) return false;
            if (!ValidationHelper.MaxLength(habitacion.Numero, 20, "Número", out errorMessage)) return false;

            // Capacidad
            if (!ValidationHelper.GreaterThanZero(habitacion.Capacidad, "Capacidad", out errorMessage)) return false;

            // Estado
            var estadosValidos = new[] { EstadoHabitacion.Disponible, EstadoHabitacion.Reservada, EstadoHabitacion.Ocupada, EstadoHabitacion.Limpieza, EstadoHabitacion.Mantenimiento}; // 0=disponible, 1=ocupada, 2=mantenimiento
            if (!ValidationHelper.InList(habitacion.Estado, estadosValidos, "Estado", out errorMessage)) return false;

            errorMessage = string.Empty;
            return true;
        }
    }
}
