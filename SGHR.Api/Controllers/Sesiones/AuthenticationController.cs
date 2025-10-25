using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SGHR.Api.Controllers.Sesiones
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            _authenticationServices = authenticationServices;
        }


        // GET: api/<SesionController>
        [HttpPost("Authentication-Register")]
        public async Task<IActionResult> RegistrarUsuarioAsync([FromBody] CreateUsuarioDto usuariodto)
        {
            ServiceResult result = new ServiceResult();
            result = await _authenticationServices.RegistrarUsuarioAsync(usuariodto);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<SesionController>
        [HttpPut("Authentication-Login")]
        public async Task<IActionResult> LoginSesionAsync(string correo, string contraseña)
        {
            ServiceResult result = new ServiceResult();
            result = await _authenticationServices.LoginSesionAsync(correo, contraseña);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        // GET: api/<SesionController>
        [HttpPut("Authentication-CloseSesion")]
        public async Task<IActionResult> CloseSerionAsync(int id)
        {
            ServiceResult result = new ServiceResult();
            result = await _authenticationServices.CloseSerionAsync(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
