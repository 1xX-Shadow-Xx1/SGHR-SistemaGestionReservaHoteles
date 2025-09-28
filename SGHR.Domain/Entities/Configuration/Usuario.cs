

using System.Diagnostics.Tracing;
using System.Numerics;

namespace SGHR.Domain.Entities.Configuration
{
    public sealed class Usuario : Base.BaseEntity
    {
        //Constructor Usuario
        public Usuario(string nombre,string correo,string contraseña,string rol)
        {
            Nombre = nombre;
            Correo = correo;
            Contraseña = contraseña;
            Rol = rol ?? "Cliente";
            Estado = "Activo";
        }

        //Logica de Usuario
        public Usuario CrearNuevoUsuario(string nombre, string correo, string contraseña,string rol)
        {
            Usuario usuario = new Usuario(nombre,correo,contraseña,rol);
            return usuario;
        }

        //Metodos para modificar atributos de Usuario
        public void CambiarEstadoUsuario(string nuevoEstado,int usuarioID)
        {
            Estado = nuevoEstado;
            RegistrarModificacion(usuarioID,"Modifico el estado del Usuario");
        }
        public void CambiarPasswordUsuario(string nuevaContraseña, int usuarioID)
        {
            Contraseña = nuevaContraseña;
            RegistrarModificacion(usuarioID,"Modifico la contraseña del Usuario");
        }
        public void CambiarRolUsuario(string nuevoRol, int usuarioID)
        {
            Rol = nuevoRol;
            RegistrarModificacion(usuarioID,"Modifico el Rol del Usuario");
        }
        public void CambiarNombreUsuario(string nuevoNombre, int usuarioID)
        {
            Nombre = nuevoNombre;
            RegistrarModificacion(usuarioID,"Modifico el Nombre del Usuario");
        }
        public void CambiarCorreoUsuario(string nuevoCorreo, int usuarioid)
        {
            Correo = nuevoCorreo;
            RegistrarModificacion(usuarioid,"Modifico el Correo del Usuario");
        }
      
        //Propiedades del Usuario
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Contraseña {  get; private set; }
        public string Rol { get; private set; }
        public string Estado { get; private set; }
 
    }
}
