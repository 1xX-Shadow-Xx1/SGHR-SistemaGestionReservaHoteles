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
        IMantenimientoServices _mantenimientoServices;
        
        public MantenimientoController(IMantenimientoServices mantenimientoServices)
        {
            _mantenimientoServices = mantenimientoServices;
        }

        // GET: api/<MantenimientoController>
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _mantenimientoServices.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<MantenimientoController>/5
        [HttpGet("Get-By-Id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            ServiceResult result = await _mantenimientoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<MantenimientoController>
        [HttpPost("Create-Mantenimiento")]
        public async Task<IActionResult> PostAsync([FromBody] CreateMantenimientoDto createDto)
        {
            ServiceResult result = await _mantenimientoServices.CreateAsync(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<MantenimientoController>/5
        [HttpPut("Update-Mantenimiento")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateMantenimientoDto UpdateDto)
        {
            ServiceResult result = await _mantenimientoServices.UpdateAsync(UpdateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // REMOVE api/<MantenimientoController>/5
        [HttpPut("Remove-Mantenimiento")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ServiceResult result = await _mantenimientoServices.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
