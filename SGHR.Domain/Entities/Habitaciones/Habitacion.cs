namespace SGHR.Domain.Entities.Hotel
{
    public sealed class Habitacion : Base.BaseEntity
    {
        //Constructor de Habitacion
        public Habitacion(int numeroHabitacion,int capacidad)
        {
            NumeroHabitacion = numeroHabitacion;
            Capacidad = capacidad;
        }
        //Logica de Habitacion

        //Metodos para modificar los atributos de la Habitacion
        public void CambiarNumeroHabitacion(int nuevoNumero,int usuarioID)
        {
            NumeroHabitacion =  nuevoNumero;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarCapacidadHabitacion(int nuevaCapacidad, int usuarioID)
        {
            Capacidad = nuevaCapacidad;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de la Habitacion
        public int NumeroHabitacion { get; private set; }
        public int Capacidad { get; private set; }
    }
}
