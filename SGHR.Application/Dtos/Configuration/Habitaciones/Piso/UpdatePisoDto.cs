using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Habitaciones.Piso
{
    public record UpdatePisoDto
    {
        public int Id { get; set; }
        public int NumeroPiso { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }
}
