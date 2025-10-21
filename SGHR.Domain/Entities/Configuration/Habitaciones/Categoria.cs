using SGHR.Domain.Entities.Configuration.Reservas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    [Table("Categorias")]
    public sealed class Categoria : Base.BaseEntity
    {
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }

    }
}
