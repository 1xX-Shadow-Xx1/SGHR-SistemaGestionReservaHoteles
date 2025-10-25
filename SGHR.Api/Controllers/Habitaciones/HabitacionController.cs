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
        public readonly IHabitacionServices _habitacionService;
        public HabitacionController(IHabitacionServices habitacionService)
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

        // GET: api/<HabitacionController>
        [HttpGet("Get-Habitaciones-disponibles")]
        public async Task<IActionResult> GetAllDisponiblesAsync()
        {
            ServiceResult result = await _habitacionService.GetAllDisponiblesAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<HabitacionController>
        [HttpGet("Get-Habitaciones-disponibles-date")]
        public async Task<IActionResult> GetAllDisponibleDateAsync(DateTime fechainicio, DateTime fechafin)
        {
            ServiceResult result = await _habitacionService.GetAllDisponibleDateAsync(fechainicio, fechafin);
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
        public async Task<IActionResult> PostAsync([FromBody] CreateHabitacionDto createDto)
        {
            ServiceResult result = await _habitacionService.CreateAsync(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // PUT api/<HabitacionController>/5
        [HttpPut("Update-Habitacion")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateHabitacionDto updateDto)
        {
            ServiceResult result = await _habitacionService.UpdateAsync(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // DELETE api/<HabitacionController>/5
        [HttpDelete("Delete-Habitacion")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ServiceResult result = await _habitacionService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
