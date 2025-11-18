using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Reservas
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        public readonly IReservaServices _reservaService;
        public ReservaController(IReservaServices reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/<ReservaController>
        [HttpGet("Get-Reservas")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _reservaService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ReservaController>/5
        [HttpGet("Get-Reserva-ByID")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            ServiceResult result = await _reservaService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // GET api/<ReservaController>/5
        [HttpGet("Get-Servicios-By-ReservaID")]
        public async Task<IActionResult> GetServicioByReservaIDAsync(int id)
        {
            ServiceResult result = await _reservaService.GetServiciosByReservaId(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ReservaController>
        [HttpPost("Create-Reserva")]
        public async Task<IActionResult> PostAsync([FromBody] CreateReservaDto createDto)
        {
            ServiceResult result = await _reservaService.CreateAsync(createDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ReservaController>/5
        [HttpPut("Update-Reserva")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateReservaDto updateDto)
        {
            ServiceResult result = await _reservaService.UpdateAsync(updateDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }

        // PUT api/<ReservaController>/5
        [HttpPut("Add-Servicio-Adicional-to-Reserva")]
        public async Task<IActionResult> AddServicioAdicional(int id, string nameServicio)
        {
            ServiceResult result = await _reservaService.AddServicioAdicional(id, nameServicio);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Remove api/<ReservaController>/5
        [HttpPut("Remove-Servicio-Adicional-to-Reserva")]
        public async Task<IActionResult> RemoveServicioAdicional(int id, string nombreServicio)
        {
            ServiceResult result = await _reservaService.RemoveServicioAdicional(id, nombreServicio);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // Remove api/<ReservaController>/5
        [HttpPut("Remove-Reserva")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ServiceResult result = await _reservaService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }            
            return Ok(result);
        }
    }
}
