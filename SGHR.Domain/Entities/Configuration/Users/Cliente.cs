using SGHR.Domain.Entities.Configuration.Reservas;
using System.Reflection.Metadata.Ecma335;

namespace SGHR.Domain.Entities.Configuration.Usuers
{
    public sealed class Cliente : Base.BaseEntity
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public Usuario Usuario { get; set; }
        public ICollection<Reserva> Reservas { get; set; }



    }
}
