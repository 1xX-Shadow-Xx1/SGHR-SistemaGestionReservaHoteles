using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Auditory;
using SGHR.Application.Interfaces.Operaciones;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditoryController : ControllerBase
    {
        public readonly IAuditoryService _auditoryService;
        public AuditoryController(IAuditoryService auditoryService)
        {
            _auditoryService = auditoryService;
        }

        // GET: api/<AuditoryController>
        [HttpGet("Get-Auditorias")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _auditoryService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<AuditoryController>/5
        [HttpGet("Get-Auditoria-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _auditoryService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<AuditoryController>
        [HttpPost("Create-Auditoria")]
        public async Task<IActionResult> PostAsync([FromBody] CreateAuditoryDto createDto, int? idsesion = null)
        {
            ServiceResult result = await _auditoryService.CreateAsync(createDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<AuditoryController>/5
        [HttpPut("Update-Auditoria")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateAuditoryDto updateDto, int? idsesion = null)
        {
            ServiceResult result = await _auditoryService.UpdateAsync(updateDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<AuditoryController>/5
        [HttpDelete("Delete-Auditoria")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _auditoryService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
