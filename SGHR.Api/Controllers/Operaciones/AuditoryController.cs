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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _auditoryService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<AuditoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _auditoryService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<AuditoryController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAuditoryDto createDto)
        {
            ServiceResult result = await _auditoryService.Save(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<AuditoryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] UpdateAuditoryDto updateDto)
        {
            ServiceResult result = await _auditoryService.Update(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<AuditoryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromBody] DeleteAuditoryDto deleteAuditoryDto)
        {
            ServiceResult result = await _auditoryService.Remove(deleteAuditoryDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
