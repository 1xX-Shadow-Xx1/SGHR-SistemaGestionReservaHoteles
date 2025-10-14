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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _habitacionService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // GET api/<HabitacionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _habitacionService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<HabitacionController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateHabitacionDto createDto)
        {
            ServiceResult result = await _habitacionService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // PUT api/<HabitacionController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateHabitacionDto updateDto)
        {
            ServiceResult result = await _habitacionService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // DELETE api/<HabitacionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] DeleteHabitacionDto deleteDto)
        {
            ServiceResult result = await _habitacionService.Remove(deleteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
