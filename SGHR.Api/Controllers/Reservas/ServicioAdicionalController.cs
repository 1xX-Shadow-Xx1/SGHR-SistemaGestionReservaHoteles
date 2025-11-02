using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioAdicionalController : ControllerBase
    {
        private readonly IServicioAdicionalServices _servicioAdicional;

        public ServicioAdicionalController(IServicioAdicionalServices servicioAdicionalServices)
        {
            _servicioAdicional = servicioAdicionalServices;
        }

        // GET: api/<ServicioAdicionalController>
        [HttpGet("Get-Servicio-Adicional")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _servicioAdicional.GetAllAsync();
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ServicioAdicionalController>/5
        [HttpGet("Get-Servicio-Adicional-By-ID")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _servicioAdicional.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ServicioAdicionalController>
        [HttpPost("Create-Servicio-Adicional")]
        public async Task<IActionResult> Post([FromBody] CreateServicioAdicionalDto servicioAdicionalDto)
        {
            ServiceResult result = await _servicioAdicional.CreateAsync(servicioAdicionalDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ServicioAdicionalController>/5
        [HttpPut("Update-Servicio-Adicional")]
        public async Task<IActionResult> Put([FromBody] UpdateServicioAdicionalDto updateServicioAdicionalDto)
        {
            ServiceResult result = await _servicioAdicional.UpdateAsync(updateServicioAdicionalDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // REMOVE api/<ServicioAdicionalController>/5
        [HttpPut("Remove-Servicio-Adicional")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _servicioAdicional.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
