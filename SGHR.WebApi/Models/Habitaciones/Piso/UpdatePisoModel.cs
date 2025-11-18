using SGHR.Web.Models.EnumsModel.Habitaciones;

namespace SGHR.Web.Models.Habitaciones.Piso
{
    public record UpdatePisoModel
    {
        public int Id { get; set; }
        public int NumeroPiso { get; set; }
        public string? Descripcion { get; set; }
        public EstadoPisoModel Estado { get; set; }
    }
}
