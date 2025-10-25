using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Users;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        public readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/<ClienteController>
        [HttpGet("Get-Clientes")]
        public async Task<IActionResult> GetAsync()
        {
            ServiceResult result = await _clienteService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ClienteController>/5
        [HttpGet("Get-Cliente-ByID")]
        public async Task<IActionResult> GetIdAsync(int id)
        {
            ServiceResult result = await _clienteService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ClienteController>
        [HttpGet("Get-Cliente-ByCedula")]
        public async Task<IActionResult> GetClientByCedulaAsync(string cedula)
        {
            ServiceResult result = await _clienteService.GetByCedulaAsync(cedula);
            if(!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ClienteController>
        [HttpPost("Create-Cliente")]
        public async Task<IActionResult> PostAsync([FromBody] CreateClienteDto createClienteDto, int? idsesion = null)
        {
            ServiceResult result = await _clienteService.CreateAsync(createClienteDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ClienteController>/5
        [HttpPut("Update-Cliente")]
        public async Task<IActionResult> PutAsync([FromBody] UpdateClienteDto updateClienteDto, int? idsesion = null)
        {
            ServiceResult result = await _clienteService.UpdateAsync(updateClienteDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("Delete-Cliente")]
        public async Task<IActionResult> DeleteAsync(int id, int? idsesion = null)
        {
            ServiceResult result = await _clienteService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }
    }
}
