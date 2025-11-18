using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public class CreatePisoDto
    {
        public int NumeroPiso { get; set; }
        public string Descripcion { get; set; }

    }
}
