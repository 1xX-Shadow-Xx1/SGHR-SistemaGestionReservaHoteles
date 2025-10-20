using SGHR.Domain.Entities.Configuration.Reservas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGHR.Domain.Entities.Configuration.Usuers
{
    [Table("Clientes")]
    public sealed class Cliente : Base.BaseEntity
    {
        [Column("id_usuario")]
        [ForeignKey(nameof(Usuario))]
        public int IdUsuario { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("apellido")]
        public string Apellido { get; set; }
        [Column("cedula")]
        public string Cedula { get; set; }
        [Column("telefono")]
        public string Telefono { get; set; }
        [Column("direccion")]
        public string Direccion { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Reserva> Reservas { get; set; }

    }
}
