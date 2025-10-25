using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Persistence.Interfaces.Reservas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioAdicionalController : ControllerBase
    {
        public readonly IServicioAdicionalService _servicioAdicionalService;
        public ServicioAdicionalController(IServicioAdicionalService servicioAdicionalService)
        {
            _servicioAdicionalService = servicioAdicionalService;
        }

        // GET: api/<ServicioAdicionalController>
        [HttpGet("Get-ServiciosAdicionales")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _servicioAdicionalService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ServicioAdicionalController>/5
        [HttpGet("Get-ServicioAdicional-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _servicioAdicionalService.GetByIdAsync(id);
            if (result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<ServicioAdicionalController>
        [HttpPost("Create-ServicioAdicional")]
        public async Task<IActionResult> PostAsync([FromBody] CreateServicioAdicionalDto createServicioAdicionalDto, int? idseison = null)
        {
            ServiceResult result = await _servicioAdicionalService.CreateAsync(createServicioAdicionalDto, idseison);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ServicioAdicionalController>/5
        [HttpPut("Update-ServicioAdicional")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateServicioAdicionalDto updateServicioAdicionalDto, int? idsesion = null)
        {
            ServiceResult result = await _servicioAdicionalService.UpdateAsync(updateServicioAdicionalDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }

        // DELETE api/<ServicioAdicionalController>/5
        [HttpDelete("Delete-ServicioAdicional")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _servicioAdicionalService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }
    }
}
