using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Reservas
{
    [Table("Servicios_Adicionales")]
    public sealed class ServicioAdicional : Base.BaseEntity
    {
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("descripcion")] 
        public string Descripcion { get; set; }
        [Column("precio")]
        public decimal Precio { get; set; }

    }
}
