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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _pagoService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<PagoController>/5
        [HttpGet("Get-Pago-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _pagoService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<PagoController>
        [HttpPost("Create-Pago")]
        public async Task<IActionResult> Post([FromBody] CreatePagoDto createDto)
        {
            ServiceResult result = await _pagoService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<PagoController>/5
        [HttpPut("Update-Pago")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePagoDto updateDto)
        {
            ServiceResult result = await _pagoService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<PagoController>/5
        [HttpDelete("Delete-Pago")]
        public async Task<IActionResult> Delete([FromBody] DeletePagoDto deletePagoDto)
        {
            ServiceResult result = await _pagoService.Remove(deletePagoDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
