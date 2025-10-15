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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _pisoService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // GET api/<PisoController>/5
        [HttpGet("Get-Piso-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _pisoService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<PisoController>
        [HttpPost("Create-Piso")]
        public async Task<IActionResult> Post([FromBody] CreatePisoDto createDto)
        {
            ServiceResult result = await _pisoService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<PisoController>/5
        [HttpPut("Update-Piso")]
        public async Task<IActionResult> Put([FromBody] UpdatePisoDto updateDto)
        {
            ServiceResult result = await _pisoService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<PisoController>/5
        [HttpDelete("Delete-Piso")]
        public async Task<IActionResult> Delete([FromBody] DeletePisoDto deletePisoDto)
        {
            ServiceResult result = await _pisoService.Remove(deletePisoDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
