using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
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
            result = await _sesionservices.GetSesion();
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
            result = await _sesionservices.GetSesionByUsers(correo);
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
            result = await _sesionservices.GetOpenSesion();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
