using SGHR.Domain.Base;
using SGHR.Domain.Entities.Configuration.Habitaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Domain.Entities.Configuration.Reservas
{
    public sealed class Tarifa : BaseEntity
    {
        public int IdCategoria { get; set; }
        public string Temporada { get; set; }
        public decimal Precio { get; set; }

        public Categoria Categoria { get; set; }

    }
}
