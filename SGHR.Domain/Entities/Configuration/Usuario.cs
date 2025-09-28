

using System.Diagnostics.Tracing;

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

        //Metodos para modificar atributos de Usuario
        public void CambiarEstadoUsuario(string nuevoEstado,int usuarioID)
        {
            Estado = nuevoEstado;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarPasswordUsuario(string nuevaContraseña, int usuarioID)
        {
            Contraseña = nuevaContraseña;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarRolUsuario(string nuevoRol, int usuarioID)
        {
            Rol = nuevoRol;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarNombreUsuario(string nuevoNombre, int usuarioID)
        {
            Nombre = nuevoNombre;
            RegistrarModificacion(usuarioID);
        }
        public void CambiarCorreoUsuario(string nuevoCorreo, int usuarioid)
        {
            Correo = nuevoCorreo;
            RegistrarModificacion(usuarioid);
        }
      
        //Propiedades del Usuario
        public string Nombre { get; private set; }
        public string Correo { get; private set; }
        public string Contraseña {  get; private set; }
        public string Rol { get; private set; }
        public string Estado { get; private set; }
 
    }
}
