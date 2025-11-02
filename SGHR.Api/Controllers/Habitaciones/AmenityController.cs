using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;
using SGHR.Application.Interfaces.Habitaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Habitaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityServices _services;

        public AmenityController(IAmenityServices services)
        {
            _services = services;
        }

        // GET: api/<AmenityController>
        [HttpGet("Get-Amenity")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _services.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<AmenityController>/5
        [HttpGet("Get-By-ID")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _services.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<AmenityController>
        [HttpPost("Create-Amenity")]
        public async Task<IActionResult> Post([FromBody] CreateAmenityDto createAmenity)
        {
            ServiceResult result = await _services.CreateAsync(createAmenity);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<AmenityController>/5
        [HttpPut("Update-Amenity")]
        public async Task<IActionResult> Put([FromBody] UpdateAmenityDto updateAmenity)
        {
            ServiceResult result = await _services.UpdateAsync(updateAmenity);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // REMOVE api/<AmenityController>/5
        [HttpPut("Remove-Amenity")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _services.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
