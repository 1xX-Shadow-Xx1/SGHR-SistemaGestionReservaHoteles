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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _checkInOutService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<CheckInOutController>/5
        [HttpGet("Get-CheckInOut-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _checkInOutService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<CheckInOutController>
        [HttpPost("Create-CheckInOut")]
        public async Task<IActionResult> Post([FromBody] CreateCheckInOutDto createDto)
        {
            ServiceResult result = await _checkInOutService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<CheckInOutController>/5
        [HttpPut("Update-CheckInOut")]
        public async Task<IActionResult> Put([FromBody] UpdateCheckInOutDto updateDto)
        {
            ServiceResult result = await _checkInOutService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<CheckInOutController>/5
        [HttpDelete("Delete-CheckInOut")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _checkInOutService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
