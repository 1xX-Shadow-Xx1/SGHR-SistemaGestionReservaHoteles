using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarifaController : ControllerBase
    {
        public readonly ITarifaServices _tarifaService;
        public TarifaController(ITarifaServices tarifaService)
        {
            _tarifaService = tarifaService;
        }

        // GET: api/<TarifaController>
        [HttpGet("Get-Tarifas")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _tarifaService.GetAllAsync();

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<TarifaController>/5
        [HttpGet("Get-Tarifa-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _tarifaService.GetByIdAsync(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<TarifaController>
        [HttpPost("Create-Tarifa")]
        public async Task<IActionResult> PostAsync([FromBody] CreateTarifaDto createTarifaDto)
        {
            ServiceResult result = await _tarifaService.CreateAsync(createTarifaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<TarifaController>/5
        [HttpPut("Update-Tarifa")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateTarifaDto updateTarifaDto)
        {
            ServiceResult result = await _tarifaService.UpdateAsync(updateTarifaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // REMOVE api/<TarifaController>/5
        [HttpPut("Remove-Tarifa")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ServiceResult result = await _tarifaService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
