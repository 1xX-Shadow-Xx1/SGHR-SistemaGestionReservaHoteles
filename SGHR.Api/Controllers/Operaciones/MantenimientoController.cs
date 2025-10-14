using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento;
using SGHR.Application.Interfaces.Operaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class MantenimientoController : ControllerBase
    {
        public readonly IMantenimientoService _mantenimientoService;
        public MantenimientoController(IMantenimientoService mantenimientoService)
        {
            _mantenimientoService = mantenimientoService;
        }

        // GET: api/<MantenimientoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _mantenimientoService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }   
            return Ok(result);
        }

        // GET api/<MantenimientoController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _mantenimientoService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<MantenimientoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMantenimientoDto createDto)
        {
            ServiceResult result = await _mantenimientoService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<MantenimientoController>/5
        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody] UpdateMantenimientoDto updateDto)
        {
            ServiceResult result = await _mantenimientoService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<MantenimientoController>/5
        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromBody] DeleteMantenimientoDto deleteDto)
        {
            ServiceResult result = await _mantenimientoService.Remove(deleteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
