using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        public readonly IReporteServices _reporteService;

        public ReporteController(IReporteServices reporteService)
        {
            _reporteService = reporteService;
        }

        // GET: api/<ReporteController>
        [HttpGet("get-Reportes")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _reporteService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ReporteController>/5
        [HttpGet("get-Reporte-ById")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _reporteService.GetByIdAsync(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ReporteController>
        [HttpPost("create-Reporte")]
        public async Task<IActionResult> PostAsync([FromBody] CreateReporteDto createReporteDto)
        {
            ServiceResult result = await _reporteService.CreateAsync(createReporteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReporteController>/5
        [HttpPut("update-Reporte")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateReporteDto updateReporteDto)
        {
            ServiceResult result = await _reporteService.UpdateAsync(updateReporteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ReporteController>/5
        [HttpDelete("delete-Reporte")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ServiceResult result = await _reporteService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
