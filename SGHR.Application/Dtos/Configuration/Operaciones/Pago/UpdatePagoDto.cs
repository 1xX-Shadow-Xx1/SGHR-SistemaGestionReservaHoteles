using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.Application.Dtos.Configuration.Operaciones.Pago
{
    public class UpdatePagoDto
    {
        public int Id { get; set; }
        public int IdReserva { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public DateTime FechaPago { get; set; }
    }
}
