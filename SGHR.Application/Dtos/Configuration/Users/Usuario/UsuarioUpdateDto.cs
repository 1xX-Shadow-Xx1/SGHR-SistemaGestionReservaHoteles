namespace SGHR.Application.Dtos.Configuration.Users.Usuario
{
    public record UsuarioUpdateDto
    {
        public int Id { get; set; }           
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
    }
}
