using SGHR.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Habitaciones
{
    public sealed class Amenities : BaseEntity
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Categoria> Categorias { get; set; }
    }
}
