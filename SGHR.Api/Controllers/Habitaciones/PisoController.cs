using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Interfaces.Habitaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Habitaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class PisoController : ControllerBase
    {
        public readonly IPisoService _pisoService;
        public PisoController(IPisoService pisoService)
        {
            _pisoService = pisoService;
        }

        // GET: api/<PisoController>
        [HttpGet("Get-Pisos")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _pisoService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // GET api/<PisoController>/5
        [HttpGet("Get-Piso-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _pisoService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<PisoController>
        [HttpPost("Create-Piso")]
        public async Task<IActionResult> PostAsync([FromBody] CreatePisoDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _pisoService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<PisoController>/5
        [HttpPut("Update-Piso")]
        public async Task<IActionResult> PutAsync([FromBody] UpdatePisoDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _pisoService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<PisoController>/5
        [HttpDelete("Delete-Piso")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _pisoService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
