using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Users;
using SGHR.Application.Services.Users;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Usuarios
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/<UsuarioController>
        [HttpGet("Get-Usuarios")]
        public async Task<IActionResult> Get()
        {
            ServiceResult result = await _usuarioService.GetAll();
            if (!result.Success)
            {
                return BadRequest(result);   
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>
        [HttpGet("Get-Usuario-Start-Sesion")]
        public async Task<IActionResult> GetLogin(string email, string password)
        {
            ServiceResult result = await _usuarioService.LoginAsync(email, password);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        // GET api/<UsuarioController>
        [HttpGet("Get-Usuario-Close-Sesion")]
        public async Task<IActionResult> CloseSesion()
        {
            ServiceResult result = await _usuarioService.CloseAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-ById")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _usuarioService.GetById(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<UsuarioController>
        [HttpPost("create-Usuario")]
        public async Task<IActionResult> Post([FromBody] UsuarioCreateDto createUsuarioDto)
        {
            ServiceResult result = await _usuarioService.Save(createUsuarioDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("update-Usuario")]
        public async Task<IActionResult> Put([FromBody] UsuarioUpdateDto updateUsuarioDto)
        {
            ServiceResult result = await _usuarioService.Update(updateUsuarioDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("delete-Usuario")]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResult result = await _usuarioService.Remove(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
