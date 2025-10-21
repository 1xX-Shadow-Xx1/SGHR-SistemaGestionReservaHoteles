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
        public readonly ITarifaService _tarifaService;
        public TarifaController(ITarifaService tarifaService)
        {
            _tarifaService = tarifaService;
        }

        // GET: api/<TarifaController>
        [HttpGet("Get-Tarifas")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _tarifaService.GetAll();

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<TarifaController>/5
        [HttpGet("Get-Tarifa-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _tarifaService.GetById(id);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<TarifaController>
        [HttpPost("Create-Tarifa")]
        public async Task<IActionResult> Post([FromBody] CreateTarifaDto createTarifaDto)
        {
            ServiceResult result = await _tarifaService.Save(createTarifaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<TarifaController>/5
        [HttpPut("Update-Tarifa")]
        public async Task<IActionResult> Put([FromBody] UpdateTarifaDto updateTarifaDto)
        {
            ServiceResult result = await _tarifaService.Update(updateTarifaDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<TarifaController>/5
        [HttpDelete("Delete-Tarifa")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _tarifaService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
