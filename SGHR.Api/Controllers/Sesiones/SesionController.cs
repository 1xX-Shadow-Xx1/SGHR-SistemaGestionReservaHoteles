using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Interfaces.Sesion;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Sesiones
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionController : ControllerBase
    {
        private readonly ISesionServices _sesionServices;

        public SesionController(ISesionServices sesionServices)
        {
            _sesionServices = sesionServices;
        }

        // GET: api/<SesionController>
        [HttpGet("GetSesionByUser")]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var result = await _sesionServices.GetSesionByIdUser(userId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // Put api/<SesionController>/5
        [HttpPut("PutCloseSesionByUserID")]
        public async Task<IActionResult> PostCloseSesionByUserID(int userId)
        {
            var result = await _sesionServices.CloseSesionAsync(userId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // Get api/<SesionController>
        [HttpGet("CheckSesionActivityByUserID")]
        public async Task<IActionResult> CheckSesionActivityByUserID(int userId)
        {
            var result = await _sesionServices.CheckActivitySesionByUserAsync(userId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        // PUT api/<SesionController>/5
        [HttpPut("UpdateActivitySesionByUser")]
        public async Task<IActionResult> UpdateActivitySesionByUser(int userId)
        {
            var result = await _sesionServices.UpdateActivitySesionByUserAsync(userId);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
    }
}
