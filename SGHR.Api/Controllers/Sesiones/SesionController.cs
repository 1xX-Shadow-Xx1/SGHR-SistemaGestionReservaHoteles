using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Interfaces.Sesiones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Sesiones
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionServices _sesionservices;
        public SesionController(ISesionServices sesionservices)
        {
            _sesionservices = sesionservices;
        }


        // GET: api/<SesionController>
        [HttpGet("Get-Sesiones")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.GetSesionAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<SesionController>
        [HttpGet("Get-Sesiones-By-Usuario")]
        public async Task<IActionResult> GetSesionByUser(string correo)
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.GetSesionByUsersAsync(correo);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<SesionController>
        [HttpGet("Get-Sesiones-By-Activas")]
        public async Task<IActionResult> GetOpenSesions()
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.GetOpenSesionAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<SesionController>
        [HttpGet("Get-Sesion-by-ID")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POS: api/<SesionController>
        [HttpPost("Open-Sesion")]
        public async Task<IActionResult> PostAsync(StartSesionDto startSesionDto)
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.OpenSesionAsync(startSesionDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT: api/<SesionController>
        [HttpPut("Close-Sesion")]
        public async Task<IActionResult> PutAsync(CloseSesionDto closeSesionDto, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.CloseSesionAsync(closeSesionDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE: api/<SesionController>
        [HttpDelete("Delete-Sesion")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = new ServiceResult();
            result = await _sesionservices.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }        
    }
}
