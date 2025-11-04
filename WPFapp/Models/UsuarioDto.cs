
namespace WPFapp.Models
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null;
        public string Correo { get; set; } = null;
        public string Rol { get; set; } = null;
        public string Estado { get; set; } = null;
    }
}
