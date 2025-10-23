namespace SGHR.Application.Dtos.Configuration.Users.Cliente
{
    public class CreateClienteDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
