using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.CheckInOut;
using SGHR.Application.Interfaces.Operaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInOutController : ControllerBase
    {
        public readonly ICheckInOutService _checkInOutService;
        public CheckInOutController(ICheckInOutService checkInOutService)
        {
            _checkInOutService = checkInOutService;
        }

        // GET: api/<CheckInOutController>
        [HttpGet("Get-CheckInOuts")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _checkInOutService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<CheckInOutController>/5
        [HttpGet("Get-CheckInOut-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _checkInOutService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<CheckInOutController>
        [HttpPost("Create-CheckInOut")]
        public async Task<IActionResult> PostAsync([FromBody] CreateCheckInOutDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _checkInOutService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<CheckInOutController>/5
        [HttpPut("Update-CheckInOut")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateCheckInOutDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _checkInOutService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<CheckInOutController>/5
        [HttpDelete("Delete-CheckInOut")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _checkInOutService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
