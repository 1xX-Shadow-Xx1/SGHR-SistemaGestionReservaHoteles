using SGHR.Domain.Enum.Habitaciones;
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public record UpdatePisoDto
    {
        public int Id { get; set; }
        public int NumeroPiso { get; set; }
        public string? Descripcion { get; set; }
        public EstadoPiso Estado { get; set; }
    }
}
