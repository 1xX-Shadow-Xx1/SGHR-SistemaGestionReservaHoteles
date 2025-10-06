using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Reservas.Tarifa
{
    public class CreateTarifaDto
    {
        public int IdCategoria { get; set; }
        public string Temporada { get; set; }
        public decimal Precio { get; set; }
    }
}
