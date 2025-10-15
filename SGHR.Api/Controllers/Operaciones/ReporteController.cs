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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _reporteService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ReporteController>/5
        [HttpGet("get-Reporte-ById")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _reporteService.GetById(id);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ReporteController>
        [HttpPost("create-Reporte")]
        public async Task<IActionResult> Post([FromBody] CreateReporteDto createReporteDto)
        {
            ServiceResult result = await _reporteService.Save(createReporteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReporteController>/5
        [HttpPut("update-Reporte")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateReporteDto updateReporteDto)
        {
            ServiceResult result = await _reporteService.Update(updateReporteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ReporteController>/5
        [HttpDelete("delete-Reporte")]
        public async Task<IActionResult> Remove([FromBody]DeleteReporteDto deleteReporteDto)
        {
            ServiceResult result = await _reporteService.Remove(deleteReporteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
