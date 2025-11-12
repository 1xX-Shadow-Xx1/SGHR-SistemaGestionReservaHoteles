using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Dtos.Configuration.Sesiones.Sesion;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces;
using SGHR.Application.Interfaces.Sesion;

namespace SGHR.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationServices _authServices;
        private readonly ISesionServices _sesionServices;

        public AuthenticationController(IAuthenticationServices authServices,
                                        ISesionServices sesionServices)
        {
            _authServices = authServices;
            _sesionServices = sesionServices;
        }

        // En un controlador
        public IActionResult Index()
        {
            return View();
        }


        // 🔹 GET: /Authentication/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🔹 POST: /Authentication/Login
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Debe ingresar correo y contraseña.";
                return View();
            }

            var result = await _authServices.LoginSesionAsync(correo, contraseña);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View();
            }

            // ✅ Guardar datos de usuario en sesión y la sesion activa
            var usuario = result.Data as UsuarioDto;

            var sesionResult = await _sesionServices.GetSesionByIdUser(usuario.Id);
            if (!sesionResult.Success)
            {
                TempData["Error"] = sesionResult.Message;
                return View();
            }
            var sesion = sesionResult.Data as SesionDto;

            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetInt32("SesionId", sesion.Id);
            HttpContext.Session.SetString("UserName", usuario.Nombre);
            HttpContext.Session.SetString("UserRole", usuario.Rol.ToString());

            TempData["Success"] = result.Message;
            // Redirigir según el rol
            switch (usuario.Rol.ToString())
            {
                case "Cliente":
                    return RedirectToAction("Index", "Home", new { area = "Cliente" });
                case "Recepcionista":
                    return RedirectToAction("Index", "Home", new { area = "Recepcionista" });
                case "Administrador":
                    return RedirectToAction("Index", "Home", new { area = "Administrador" });
                default:
                    return RedirectToAction("Login");
            }

        }

        // 🔹 GET: /Authentication/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new CreateUsuarioDto());
        }

        // 🔹 POST: /Authentication/Register
        [HttpPost]
        public async Task<IActionResult> Register(CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _authServices.RegistrarUsuarioAsync(dto);

            if (!result.Success)
            {
                ViewBag.Error = result.Message;
                return View(dto);
            }

            TempData["Success"] = result.Message;
            return RedirectToAction("Login");
        }

        // 🔹 GET: /Authentication/Logout
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
                await _authServices.CloseSesionAsync(userId.Value);

            HttpContext.Session.Clear();
            TempData["Success"] = "Sesión cerrada correctamente.";
            return RedirectToAction("Login");
        }
    
        [HttpPost]
        public async Task<IActionResult> AutoLogout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                await _sesionServices.CloseSesionAsync(userId.Value);
                await _authServices.CloseSesionAsync(userId.Value);
            }

            HttpContext.Session.Clear();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckSession([FromBody] CheckSesionDto dto)
        {
            var sesionResult = await _sesionServices.CheckActivitySesionByUserAsync(dto.IdUsuario);
            bool active = false;

            if (sesionResult.Success && sesionResult.Data != null)
            {
                active = sesionResult.Data.Estado; // true = activa, false = inactiva
            }
            return Ok(new { active });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivity(int idusuario)
        {
            var sesionid = HttpContext.Session.GetInt32("SesionId");

            var sesionResult = await _sesionServices.UpdateActivitySesionByUserAsync(sesionid.Value);
            bool active = false;

            if (sesionResult.Success && sesionResult.Data != null)
            {
                active = sesionResult.Data.Estado; // true = activa, false = inactiva
            }
            return Ok(new { active });
        }


    }
}
