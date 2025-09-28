namespace SGHR.Domain.Entities.Hotel
{
    public sealed class Piso : Base.BaseEntity
    {
        //Constructor de Piso
        public Piso(int numeroPiso,string descripcion)
        {
            NumeroPiso = numeroPiso;
            DescripcionPiso = descripcion;
        }
        //Logica de Piso


        //Metodos para Modificar los atributos de Piso
        public void CambiarNumeroPiso(int nuevoNumero,int usuarioID)
        {
            NumeroPiso = nuevoNumero;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarDescripcionPiso(string descripcion,int usuarioID)
        {
            DescripcionPiso = descripcion;
            RegistrarModificacion(usuarioID);
        }

        //Atributos del Piso
        public int NumeroPiso { get; private set; }
        public string DescripcionPiso { get; private set; }

    }
}
