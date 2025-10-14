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
        [HttpGet]
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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ServiceResult result = await _clienteService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<ClienteController>
        [HttpPost]
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
        [HttpPut("")]
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
        [HttpDelete("")]
        public async Task<IActionResult> Delete([FromBody] DeleteClienteDto deleteClienteDto)
        {
            ServiceResult result = await _clienteService.Remove(deleteClienteDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Data);
        }
    }
}
