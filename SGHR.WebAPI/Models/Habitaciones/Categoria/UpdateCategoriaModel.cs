namespace SGHR.Web.Models.Habitaciones.Categoria
{
    public record UpdateCategoriaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
    }
}
