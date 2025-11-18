using System.ComponentModel.DataAnnotations;

namespace SGHR.Web.Models.Habitaciones.Amenity
{
    public class CreateAmenityModel
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PorCapacidad { get; set; }
    }
}
