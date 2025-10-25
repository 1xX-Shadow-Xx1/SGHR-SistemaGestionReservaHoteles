using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;
using System.Security.Cryptography.Pkcs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        public readonly IPagoService _pagoService;
        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }


        // GET: api/<PagoController>
        [HttpGet("Get-Pagos")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _pagoService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<PagoController>/5
        [HttpGet("Get-Pago-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _pagoService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<PagoController>
        [HttpPost("Create-Pago")]
        public async Task<IActionResult> PostAsync([FromBody] CreatePagoDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _pagoService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<PagoController>/5
        [HttpPut("Update-Pago")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdatePagoDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _pagoService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<PagoController>/5
        [HttpDelete("Delete-Pago")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _pagoService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
