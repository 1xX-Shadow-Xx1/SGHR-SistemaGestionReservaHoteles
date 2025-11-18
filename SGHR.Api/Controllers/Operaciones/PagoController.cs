using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        public readonly IPagoServices _pagoservices;

        public PagoController(IPagoServices pagoServices)
        {
            _pagoservices = pagoServices;
        }

        // GET: api/<PagoController>
        [HttpGet("Get-Resumen-Pagos")]
        public async Task<IActionResult> GetResumenPagosAsync()
        {
            ServiceResult result = await _pagoservices.ObtenerResumenPagosAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<PagoController>/5
        [HttpGet("Get-Pagos")]
        public async Task<IActionResult> GetPagosAsync()
        {
            ServiceResult result = await _pagoservices.ObtenerPagosAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<PagoController>/5
        [HttpGet("Get-PagosByID")]
        public async Task<IActionResult> GetPagobyIDAsync(int id)
        {
            ServiceResult result = await _pagoservices.GetPagoByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<PagoController>
        [HttpPost("Realizar-Pago")]
        public async Task<IActionResult> PostPagoAsync([FromBody] RealizarPagoDto realizarPagoDto)
        {
            ServiceResult result = await _pagoservices.RealizarPagoAsync(realizarPagoDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<PagoController>/5
        [HttpPut("Anular-Pago")]
        public async Task<IActionResult> PutPagoAsync(int idPago)
        {
            ServiceResult result = await _pagoservices.AnularPagoAsync(idPago);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
