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
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _amenityService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<AmenityController>/5
        [HttpGet("Get-Amenity-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _amenityService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<AmenityController>
        [HttpPost("Create-Amenity")]
        public async Task<IActionResult> PostAsync([FromBody] CreateAmenityDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _amenityService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<AmenityController>/5
        [HttpPut("Update-Amenity")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAmenityDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _amenityService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<AmenityController>/5
        [HttpDelete("Delete-Amenity")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _amenityService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
