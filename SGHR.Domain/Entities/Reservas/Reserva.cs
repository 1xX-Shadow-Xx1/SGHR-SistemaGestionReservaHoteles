

using SGHR.Domain.Entities.Configuration;

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
            Estado = "Activa";
        }

        //Logica de Reserva
        public Reserva crearReserva(string fechaInicio,string fechaFin,decimal costoTotal)
        {
            Reserva reserva = new Reserva(fechaInicio, fechaFin, costoTotal);
            return reserva;
        }
        public void ModificarReserva(string? nuevaFechaInicio,string? nuevaFechaFin,decimal? nuevoCostoTotal,int usuarioID)
        {
            if (nuevaFechaInicio != null) FechaInicio = nuevaFechaInicio; 
            if (nuevaFechaFin != null) FechaFin = nuevaFechaFin;
            if (nuevoCostoTotal != null) CostoTotal = (decimal)nuevoCostoTotal;

             
        }
        public Reserva CancelarReserva(Reserva reserva)
        {
            reserva.Estado = "Cancelada";
            return reserva;
        }
        public void CalcularTotal()
        {

        }
        //Metodos para modificar atributos de Reserva
        public void CambiarFechaInicio(string fechaInicio,int usuarioID)
        {
            FechaInicio = fechaInicio;
            RegistrarModificacion(usuarioID,"Modifico la Fecha de Inicio de la Reserva");
        }

        //Atributos de Reserva
        public string FechaInicio { get; private set; }
        public string FechaFin { get; private set; }
        public decimal CostoTotal { get; private set; }
        public string Estado {  get; private set; }

    }
}
