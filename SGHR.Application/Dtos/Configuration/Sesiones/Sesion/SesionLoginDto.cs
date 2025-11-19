using SGHR.Domain.Enum.Usuarios;

namespace SGHR.Application.Dtos.Configuration.Sesiones.Sesion
{
    public class SesionLoginDto
    {
        public int IdUser { get; set; }
        public int Idsesion { get; set; }
        public string UserName { get; set; }
        public RolUsuarios RolUser { get; set; }
    }
}
