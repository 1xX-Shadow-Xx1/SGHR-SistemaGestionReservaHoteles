
using System.ComponentModel.DataAnnotations;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Categoria
{
    public class CreateCategoriaDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
    }
}
