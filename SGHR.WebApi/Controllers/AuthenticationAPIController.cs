using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.EnumsModel.Usuario;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Validador;

namespace SGHR.Web.Controllers
{
    public class AuthenticationAPIController : Controller
    {
        private readonly HttpSesion _contexSesion;
        
        public AuthenticationAPIController(HttpSesion httpSesion)
        {
            _contexSesion = httpSesion;
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
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Authentication/Authentication-Login?correo={Uri.EscapeDataString(correo)}&contraseña={Uri.EscapeDataString(contraseña)}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var sesionModel = await new JsonConvertidor<SesionLoginModel>().Deserializar(endpoint);

                    if (sesionModel != null && sesionModel.Data != null && sesionModel.Success)
                    {
                        _contexSesion.SaveSesion(sesionModel.Data);

                        TempData["Success"] = sesionModel.Message;
                        switch (sesionModel.Data.RolUser)
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
                        TempData["Error"] = sesionModel.Message;
                        return RedirectToAction("Login");

                    }
                }
            }catch(Exception ex)
            {
                ViewBag.Error = "Error interno al conectar con el servicio de autenticación.";
                return View("Error", ex);
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
        public async Task<IActionResult> Register(CreateUsuarioModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PostAsJsonAsync("Authentication/Authentication-Register", dto);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var sesionModel = await new JsonConvertidor<SesionLoginModel>().Deserializar(endpoint);

                    if (sesionModel != null && sesionModel.Data != null && sesionModel.Success)
                    {
                        _contexSesion.SaveSesion(sesionModel.Data);
                        TempData["Success"] = sesionModel.Message;
                        return RedirectToAction("Index", "HomeAPI", new { area = "Cliente" });
                    }
                    else
                    {
                        TempData["Error"] = sesionModel.Message;
                        return RedirectToAction("Login");
                    }       
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Error interno al conectar con el servicio de autenticación.";
                return View("Error");
            }
        }

        // 🔹 Put: /Authentication/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Authentication/Authentication-CloseSesion?id={userId.Value}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var sesionService = await new JsonConvertidor<object>().Deserializar(endpoint);

                    if (sesionService != null && sesionService.Success)
                    {
                        HttpContext.Session.Clear();
                        TempData["Success"] = sesionService.Message;
                        return RedirectToAction("Login");

                    }
                    else
                    {
                        TempData["Error"] = sesionService.Message;
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al conectar con el servicio de autenticación.";
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AutoLogout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                try
                {
                    using (var httpclient = new HttpClient())
                    {
                        httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                        var endpoint = await httpclient.PutAsync($"Sesion/PutCloseSesionByUserID?userId={userId.Value}", null);

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                        }

                        var sesionService = await new JsonConvertidor<object>().Deserializar(endpoint);

                        if (sesionService != null && sesionService.Success)
                        {
                            HttpContext.Session.Clear();
                            TempData["Success"] = sesionService.Message;
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            TempData["Error"] = sesionService.Message;
                            return RedirectToAction("Login");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error interno al cerrar la sesion.";
                    return View("Error");
                }
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> CheckSession(CheckSesionModel Dto)
        {
            bool active = false;
            try
            {
                using (var httpclient = new HttpClient())
                {

                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Sesion/CheckSesionActivityByUserID?userId={Dto.IdUsuario}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var sesionService = await new JsonConvertidor<CheckSesionModel>().Deserializar(endpoint);

                    if (sesionService != null && sesionService.Success && sesionService.Data != null)
                    {
                        active = sesionService.Data.Estado;
                        return Ok(new { active });
                    }
                    else
                    {
                        return Ok(new { active });
                    }

                }

            }
            catch(Exception ex)
            {
                ViewBag.Error = "Error interno al chekear la sesion.";
                return View("Error", ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActivity()
        {
            var sesionid = HttpContext.Session.GetInt32("SesionId");
            bool active = false;
            try
            {

                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Sesion/UpdateActivitySesionByUser?userId={sesionid.Value}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var sesionService = await new JsonConvertidor<CheckSesionModel>().Deserializar(endpoint);

                    if (sesionService != null && sesionService.Success && sesionService.Data != null)
                    {
                        active = sesionService.Data.Estado;
                        return Ok(new { active });// true = activa, false = inactiva
                    }
                    else
                        return Ok(new { active = false });
                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Error internoa al actualizar la actividad del usuario.";
                return View("Error");
            }
        }
    }
}
