using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Amenity
{
    public class CreateAmenityDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El precio del amenity es obligatorio.")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El precio por capacidad de habitacion del amenity es obligatorio.")]
        public decimal PorCapacidad { get; set; }
    }
}
