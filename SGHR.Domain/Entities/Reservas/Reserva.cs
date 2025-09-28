

namespace SGHR.Domain.Entities.Reserva
{
    public sealed class Reserva : Base.BaseEntity
    {
        //Constructor de Reserva
        public Reserva(string fechaInicio,string fechaFin,decimal costoTotal)
        {
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            CostoTotal = costoTotal;
        }

        //Logica de Reserva
        public void crearReserva()
        {

        }
        public void ModificarReserva()
        {

        }
        public void CancelarReserva()
        {

        }
        public void CalcularTotal()
        {

        }

        //Metodos para Modificar los atributos de Reserva
        public void CambiarFechaInicioReserva(string nuevaFechaInicio,int usuarioID)
        {
            FechaInicio = nuevaFechaInicio;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarFechaFinReserva(string nuevaFechafin, int usuarioID)
        {
            FechaFin = nuevaFechafin;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarCostoTotalReserva(decimal nuevoCostoTotal, int usuarioID)
        {
            CostoTotal = nuevoCostoTotal;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de Reserva
        public string FechaInicio { get; private set; }
        public string FechaFin { get; private set; }
        public decimal CostoTotal { get; private set; }

    }
}
