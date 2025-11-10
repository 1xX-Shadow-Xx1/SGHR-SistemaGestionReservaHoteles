
using SGHR.Domain.Enum.Habitaciones;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public class PisoDto
    {
        public int Id { get; set; }
        public int NumeroPiso { get; set; }
        public string Descripcion { get; set; }
        public EstadoPiso Estado { get; set; }
    }
}
