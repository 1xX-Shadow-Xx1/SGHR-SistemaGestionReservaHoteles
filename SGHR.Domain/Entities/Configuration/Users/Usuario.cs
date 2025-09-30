namespace SGHR.Domain.Entities.Configuration.Usuers
{
    public sealed class Usuario : Base.BaseEntity
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }   
        public string Estado { get; set; } = "Activo";
        public ICollection<Cliente> Clientes { get; set; }
    }
}
