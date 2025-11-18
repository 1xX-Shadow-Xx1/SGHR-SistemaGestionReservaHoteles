namespace SGHR.Web.Models.Habitaciones.Amenity
{
    public record UpdateAmenityModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public decimal PorCapacidad { get; set; }
    }
}
