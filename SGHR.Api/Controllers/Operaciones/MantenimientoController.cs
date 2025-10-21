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
        [HttpGet("Get-Mantenimientos")]
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
        [HttpGet("Get-Mantenimiento-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _mantenimientoService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<MantenimientoController>
        [HttpPost("Create-Mantenimiento")]
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
        [HttpPut("Update-Mantenimiento")]
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
        [HttpDelete("Delete-Mantenimiento")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _mantenimientoService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
