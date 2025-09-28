
namespace SGHR.Domain.Entities.Configuration
{
    public sealed class Cliente : Base.BaseEntity
    {
        public Cliente(string nombre, string apellido, string cedula,string telefono,string direccion)
        {
            Nombre = nombre;
            Apellido = apellido;
            Cedula = cedula;
            Telefono = telefono;
            Direccion = direccion;
        }

        //Logica de Cliente
        public Cliente CrearNuevoCliente(string nombre,string apellido,string cedula,string telefono,string direccion)
        {
            Cliente cliente = new Cliente(nombre,apellido,cedula,telefono,direccion);
            return cliente;
        }

        //Metodos para modificar atributos de Cliente
        public void CambiarNombreCliente(string nuevoNombre,int usuarioID)
        {
            Nombre = nuevoNombre;
            RegistrarModificacion(usuarioID, "Modifico el Nombre del Cliente");
        }
        public void CambiarApellidoCliente(string nuevoApellido, int usuarioID)
        {
            Apellido = nuevoApellido;
            RegistrarModificacion(usuarioID, "Modifico el Apellido del Cliente");
        }
        public void CambiarCedulaCliente(string nuevaCedula, int usuarioID)
        {
            Cedula = nuevaCedula;
            RegistrarModificacion(usuarioID, "Modifico la Cedula del Cliente");
        }
        public void CambiarTelefonoCliente(string nuevoTelefono, int usuarioID)
        {
            Telefono = nuevoTelefono;
            RegistrarModificacion(usuarioID,"Modifico el Telefono del Cliente");
        }
        public void CambiarDireccionCliente(string nuevaDireccion, int usuarioID)
        {
            Direccion = nuevaDireccion;
            RegistrarModificacion(usuarioID,"Modifico la Direccion del Cliente");
        }

        //Atributos de Cliente

        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string Cedula { get; private set; }
        public string Telefono { get; private set; }
        public string Direccion { get; private set; }
    }
}
