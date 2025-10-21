using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        public readonly IReservaService _reservaService;
        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/<ReservaController>
        [HttpGet("Get-Reservas")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _reservaService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ReservaController>/5
        [HttpGet("Get-Reserva-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _reservaService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<ReservaController>
        [HttpPost("Create-Reserva")]
        public async Task<IActionResult> Post([FromBody] CreateReservaDto createDto)
        {
            ServiceResult result = await _reservaService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReservaController>/5
        [HttpPut("Update-Reserva")]
        public async Task<IActionResult> Put([FromBody] UpdateReservaDto updateDto)
        {
            ServiceResult result = await _reservaService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // DELETE api/<ReservaController>/5
        [HttpDelete("Delete-Reserva")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _reservaService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }
    }
}
