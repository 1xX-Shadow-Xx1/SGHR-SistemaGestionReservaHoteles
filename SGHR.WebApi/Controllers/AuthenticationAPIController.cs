using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Data;
using SGHR.Web.Models.EnumsModel.Usuario;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.Interfaces.Authentification;
using SGHR.Web.Validador;

namespace SGHR.Web.Controllers
{
    public class AuthenticationAPIController : Controller
    {
        private readonly HttpSesion _contexSesion;
        private readonly IAuthentificationServiceAPI _authentificationServiceAPI;

        public AuthenticationAPIController(HttpSesion httpSesion, IAuthentificationServiceAPI authentificationServiceAPI)
        {
            _contexSesion = httpSesion;
            _authentificationServiceAPI = authentificationServiceAPI;
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

        // 🔹 Put: /Authentication/Login
        [HttpPost]
        public async Task<IActionResult> Login(string correo, string contraseña)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contraseña))
            {
                ViewBag.Error = "Debe ingresar correo y contraseña.";
                return View();
            }

            try
            {
                var result = await _authentificationServiceAPI.LoginAsync(correo, contraseña);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Data != null && result.Success)
                {
                    _contexSesion.SaveSesion(result.Data);

                    TempData["Success"] = result.Message;
                    switch (result.Data.RolUser)
                    {
                        case RolUsuarioModel.Cliente:
                            return RedirectToAction("Index", "HomeAPI", new { area = "Cliente" });
                        case RolUsuarioModel.Recepcionista:
                            return RedirectToAction("Index", "HomeAPI", new { area = "Recepcionista" });
                        case RolUsuarioModel.Administrador:
                            return RedirectToAction("Index", "HomeAPI", new { area = "Administrador" });
                        default:
                            return RedirectToAction("Login");
                    }

                }
                else
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Login");

                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }

        }

        // 🔹 Get: /Authentication/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new CreateUsuarioModel());
        }

        // 🔹 POST: /Authentication/Register
        [HttpPost]
        public async Task<IActionResult> Register(CreateUsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _authentificationServiceAPI.RegisterAsync(model);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Data != null && result.Success)
                {
                    _contexSesion.SaveSesion(result.Data);
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Index", "HomeAPI", new { area = "Cliente" });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Login");
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }
        

        // 🔹 Put: /Authentication/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _authentificationServiceAPI.CloseSesionAsync();

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        HttpContext.Session.Clear();
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Login");

                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("Login");
                    }
                
            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AutoLogout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                try
                {
                    var result = await _authentificationServiceAPI.CloseSesionAsync();

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        HttpContext.Session.Clear();
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("Login");
                    }

                }
                catch (Exception ex)
                {
                    var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
                }
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> CheckSession()
        {
            
            try
            {
                bool active = false;

                var result = await _authentificationServiceAPI.CheckSesionAsync();

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success && result.Data != null)
                {
                    active = result.Data.Estado;
                    return Ok(new { active });
                }
                else
                {
                    return Ok(new { active });
                }
            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }

        }

        [HttpGet]
        public async Task<IActionResult> UpdateActivity()
        {
            
            try
            {
                bool active = false;

                var result = await _authentificationServiceAPI.UpdateActivitySesionAsync();

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success && result.Data != null)
                {
                    active = result.Data.Estado;
                    return Ok(new { active });// true = activa, false = inactiva
                }
                else
                    return Ok(new { active = false });


            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }
    }
}
