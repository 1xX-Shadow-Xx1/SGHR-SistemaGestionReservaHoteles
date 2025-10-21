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
        public readonly IAmenityService _amenityService;
        public AmenityController(IAmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        // GET: api/<AmenityController>
        [HttpGet("Get-Amenities")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _amenityService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<AmenityController>/5
        [HttpGet("Get-Amenity-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _amenityService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<AmenityController>
        [HttpPost("Create-Amenity")]
        public async Task<IActionResult> Post([FromBody] CreateAmenityDto createDto)
        {
            ServiceResult result = await _amenityService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<AmenityController>/5
        [HttpPut("Update-Amenity")]
        public async Task<IActionResult> Put([FromBody] UpdateAmenityDto updateDto)
        {
            ServiceResult result = await _amenityService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<AmenityController>/5
        [HttpDelete("Delete-Amenity")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _amenityService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
