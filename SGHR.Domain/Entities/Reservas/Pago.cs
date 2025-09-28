namespace SGHR.Domain.Entities.Reserva
{
    public sealed class Pago : Base.BaseEntity
    {
        //Constructor de Pago
        public Pago(decimal monto,string metodoPago)
        {
            Monto = monto;
            MetodoPago = metodoPago;
        }

        //Logica de Pago


        //Metodos para Modificar los atributos de Pago
        public void CambiarMetodoPago(string nuevoMetodo,int usuarioID)
        {
            MetodoPago = nuevoMetodo;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de Pago
        public decimal Monto { get; }
        public string MetodoPago { get; private set; }

    }
}
