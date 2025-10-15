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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _servicioAdicionalService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ServicioAdicionalController>/5
        [HttpGet("Get-ServicioAdicional-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _servicioAdicionalService.GetById(id);
            if (result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // POST api/<ServicioAdicionalController>
        [HttpPost("Create-ServicioAdicional")]
        public async Task<IActionResult> Post([FromBody] CreateServicioAdicionalDto createServicioAdicionalDto)
        {
            ServiceResult result = await _servicioAdicionalService.Save(createServicioAdicionalDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ServicioAdicionalController>/5
        [HttpPut("Update-ServicioAdicional")]
        public async Task<IActionResult> Put([FromBody] UpdateServicioAdicionalDto updateServicioAdicionalDto)
        {
            ServiceResult result = await _servicioAdicionalService.Update(updateServicioAdicionalDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }

        // DELETE api/<ServicioAdicionalController>/5
        [HttpDelete("Delete-ServicioAdicional")]
        public async Task<IActionResult> Delete([FromBody] DeleteServicioAdicionalDto deleteServicioAdicionalDto)
        {
            ServiceResult result = await _servicioAdicionalService.Remove(deleteServicioAdicionalDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }
    }
}
