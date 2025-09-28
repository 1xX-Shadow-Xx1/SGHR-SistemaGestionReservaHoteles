namespace SGHR.Domain.Entities.Reservas
{
    public sealed class ServicioAdicional : Base.BaseEntity
    {
        //Constructor de ServicioAdicional
        public ServicioAdicional(string nombre,string descripcion,decimal precio)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
        }

        //Logica de ServicioAdicional


        //Metodos para Modificar los atributos de ServicioAdicional
        public void CambiarNombreServicioAdicional(string nuevoNombre, int usuarioID)
        {
            Nombre = nuevoNombre;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarDescripcionServicioAdicional(string nuevaDescripcion, int usuarioID)
        {
            Descripcion = nuevaDescripcion;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarPrecioServicioAdicional(decimal nuevoPrecio,int usuarioID)
        {
            Precio = nuevoPrecio;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de ServicioAdicional
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public decimal Precio { get; private set; }

    }
}
