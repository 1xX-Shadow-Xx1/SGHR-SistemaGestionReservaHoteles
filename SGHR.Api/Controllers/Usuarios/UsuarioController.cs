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
            ServiceResult result = await _usuarioService.GetAllAsync();
            if (!result.Success)
            {
                return BadRequest(result);   
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>
        [HttpPost("Usuario-Start-Sesion")]
        public async Task<IActionResult> LoginAsync( UsuarioLoginDto usuarioLoginDto)
        {
            ServiceResult result = await _usuarioService.LoginAsync(usuarioLoginDto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-By-Id")]
        public async Task<IActionResult> GetByID(int id)
        {
            ServiceResult result = await _usuarioService.GetByIdAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-By-correo")]
        public async Task<IActionResult> GetByCorreoAsync(string correo)
        {
            ServiceResult result = await _usuarioService.GetByCorreoAsync(correo);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-By-Rol")]
        public async Task<IActionResult> GetByRolAsync(string rol)
        {
            ServiceResult result = await _usuarioService.GetByRolAsync(rol);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-By-filter")]
        public async Task<IActionResult> GetAllByUserAsync(string? username, string? rol, string? estado)
        {
            ServiceResult result = await _usuarioService.GetAllByAsync(username,rol,estado);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("Get-Usuario-Activos")]
        public async Task<IActionResult> GetActivosAsync()
        {
            ServiceResult result = await _usuarioService.GetActivosAsync();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // POST api/<UsuarioController>
        [HttpPost("create-Usuario")]
        public async Task<IActionResult> Post([FromBody] CreateUsuarioDto createUsuarioDto, int? idsesion = null)
        {
            ServiceResult result = await _usuarioService.CreateAsync(createUsuarioDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("update-Usuario")]
        public async Task<IActionResult> Put([FromBody] UpdateUsuarioDto updateUsuarioDto, int? idsesion = null)
        {
            ServiceResult result = await _usuarioService.UpdateAsync(updateUsuarioDto, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("delete-Usuario")]
        public async Task<IActionResult> Delete(int id, int? idsesion = null)
        {
            ServiceResult result = await _usuarioService.DeleteAsync(id, idsesion);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        
    }
}
