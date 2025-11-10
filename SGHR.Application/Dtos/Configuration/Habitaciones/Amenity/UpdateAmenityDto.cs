using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Amenity
{
    public record UpdateAmenityDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El precio del amenity es obligatorio.")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El precio por capacidad de habitacion del amenity es obligatorio.")]
        public decimal PorCapacidad { get; set; }
    }
}
