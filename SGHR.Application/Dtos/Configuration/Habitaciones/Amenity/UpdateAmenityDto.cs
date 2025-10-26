namespace SGHR.Application.Dtos.Configuration.Habitaciones.Amenity
{
    public record UpdateAmenityDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
