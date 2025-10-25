using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Persistence.Interfaces.Reportes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        public readonly IReporteService _reporteService;

        public ReporteController(IReporteService reporteService)
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
        public async Task<IActionResult> PostAsync([FromBody] CreateReporteDto createReporteDto, int? idsesion = null)
        {
            ServiceResult result = await _reporteService.CreateAsync(createReporteDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReporteController>/5
        [HttpPut("update-Reporte")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateReporteDto updateReporteDto, int? idsesion = null)
        {
            ServiceResult result = await _reporteService.UpdateAsync(updateReporteDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ReporteController>/5
        [HttpDelete("delete-Reporte")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _reporteService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
