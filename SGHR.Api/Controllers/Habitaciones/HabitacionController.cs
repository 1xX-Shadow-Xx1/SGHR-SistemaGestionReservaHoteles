using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Interfaces.Habitaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Habitaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionController : ControllerBase
    {
        public readonly IHabitacionService _habitacionService;
        public HabitacionController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        // GET: api/<HabitacionController>
        [HttpGet("Get-Habitaciones")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _habitacionService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // GET api/<HabitacionController>/5
        [HttpGet("Get-Habitacion-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _habitacionService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<HabitacionController>
        [HttpPost("Create-Habitacion")]
        public async Task<IActionResult> PostAsync([FromBody] CreateHabitacionDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _habitacionService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // PUT api/<HabitacionController>/5
        [HttpPut("Update-Habitacion")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateHabitacionDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _habitacionService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // DELETE api/<HabitacionController>/5
        [HttpDelete("Delete-Habitacion")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _habitacionService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
