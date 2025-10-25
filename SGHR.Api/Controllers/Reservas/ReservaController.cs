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
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _reservaService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ReservaController>/5
        [HttpGet("Get-Reserva-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _reservaService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<ReservaController>
        [HttpPost("Create-Reserva")]
        public async Task<IActionResult> PostAsync([FromBody] CreateReservaDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _reservaService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReservaController>/5
        [HttpPut("Update-Reserva")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateReservaDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _reservaService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // DELETE api/<ReservaController>/5
        [HttpDelete("Delete-Reserva")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _reservaService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }
    }
}
