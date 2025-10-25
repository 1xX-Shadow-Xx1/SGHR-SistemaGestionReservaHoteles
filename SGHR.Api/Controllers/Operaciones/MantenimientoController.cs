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
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _mantenimientoService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }   
            return Ok(result);
        }

        // GET api/<MantenimientoController>/5
        [HttpGet("Get-Mantenimiento-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _mantenimientoService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<MantenimientoController>
        [HttpPost("Create-Mantenimiento")]
        public async Task<IActionResult> PostAsync([FromBody] CreateMantenimientoDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _mantenimientoService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<MantenimientoController>/5
        [HttpPut("Update-Mantenimiento")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateMantenimientoDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _mantenimientoService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<MantenimientoController>/5
        [HttpDelete("Delete-Mantenimiento")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _mantenimientoService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
