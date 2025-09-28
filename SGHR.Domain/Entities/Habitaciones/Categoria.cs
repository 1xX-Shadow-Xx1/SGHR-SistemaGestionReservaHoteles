namespace SGHR.Domain.Entities.Hotel
{
    public sealed class Categoria : Base.BaseEntity
    {
        //Constructor
        public Categoria(string nombre,string descripcion,decimal tarifabase)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Tarifabase = tarifabase;
        }
        //Logica de Categoria

        //Metodos para modificar atributos de Categoria
        public void CambiarNombreCategoria(string nuevoNombre,int usuarioID)
        {
            Nombre = nuevoNombre;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarDescripcionCategoria(string nuevaDescripcion, int usuarioID)
        {
            Descripcion = nuevaDescripcion;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarTarifaBaseCategoria(decimal nuevaTarifaBase, int usuarioID)
        {
            Tarifabase = nuevaTarifaBase;
            RegistrarModificacion(usuarioID);
        }

        //Atributos de Categoria
        public string Nombre { get; private set; }
        public string Descripcion { get; private set; }
        public decimal Tarifabase { get; private set; }
    }
}
