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
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _clienteService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ClienteController>/5
        [HttpGet("Get-Cliente-ByID")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _clienteService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<ClienteController>
        [HttpGet("Get-Cliente-ByCedula")]
        public async Task<IActionResult> GetClientByCedula(string cedula)
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
        public async Task<IActionResult> Post([FromBody] CreateClienteDto createClienteDto)
        {
            ServiceResult result = await _clienteService.Save(createClienteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<ClienteController>/5
        [HttpPut("Update-Cliente")]
        public async Task<IActionResult> Put([FromBody] UpdateClienteDto updateClienteDto)
        {
            ServiceResult result = await _clienteService.Update(updateClienteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("Delete-Cliente")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _clienteService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }
    }
}
