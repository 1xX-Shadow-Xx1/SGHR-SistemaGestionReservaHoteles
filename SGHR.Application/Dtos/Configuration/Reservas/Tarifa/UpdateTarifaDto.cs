using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public record UpdateTarifaDto
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Temporada { get; set; }
        public decimal Precio { get; set; }
    }
}
