using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.EnumsModel.Usuario;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;

namespace SGHR.Web.Controllers
{
    public class AuthenticationAPIController : Controller
    {
        
        public AuthenticationAPIController()
        {
            
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
                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var userService = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);
                        if (userService != null && userService.Data != null && userService.Success)
                        {
                            var endpoint2 = await httpclient.GetAsync($"Sesion/GetSesionByUser?userId={userService.Data.Id}");
                            if (endpoint2.IsSuccessStatusCode)
                            {
                                string response2 = await endpoint2.Content.ReadAsStringAsync();
                                var sesionService = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(response2);
                                if (sesionService != null && sesionService.Data != null && sesionService.Success)
                                {

                                    HttpContext.Session.SetInt32("UserId", userService.Data.Id);
                                    HttpContext.Session.SetInt32("SesionId", sesionService.Data.Idsesion);
                                    HttpContext.Session.SetString("UserName", userService.Data.Nombre);
                                    HttpContext.Session.SetString("UserRole", (userService.Data.Rol).ToString());
                                    TempData["Success"] = userService.Message;
                                    switch (userService.Data.Rol)
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
                            }
                            string res = await endpoint.Content.ReadAsStringAsync();
                            var resul = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(res);
                            TempData["Error"] = resul.Message;
                            return RedirectToAction("Login");

                        }
                        else
                        {
                            TempData["Error"] = userService.Message;
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resul = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);
                        TempData["Error"] = resul.Message;
                        return RedirectToAction("Login");
                    }
                }

            }catch(Exception ex)
            {
                ViewBag.Error = "Error interno al conectar con el servicio de autenticación.";
                return View("Error");
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
                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var userService = JsonConvert.DeserializeObject<ServicesResultModel<CreateUsuarioModel>>(response);
                        if (userService != null && userService.Success)
                        {
                            TempData["Success"] = userService.Message;
                            return RedirectToAction("Login");

                        }
                        
                        ViewBag.Error = userService.Message;
                        return View(dto);
                    }
                    else if (400 == (int)endpoint.StatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resul = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);
                        ViewBag.Error = resul.Message;
                        return View(dto);
                    }
                    else
                    {
                        ViewBag.Error = $"Error Code: {(int)endpoint.StatusCode}";
                        return View(dto);
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
                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var sesionService = JsonConvert.DeserializeObject<ServicesResultModel<CheckSesionModel>>(response);
                        if (sesionService != null && sesionService.Success)
                        {
                            HttpContext.Session.Clear();
                            TempData["Success"] = "Sesión cerrada correctamente.";
                            return RedirectToAction("Login");

                        }
                        TempData["Error"] = sesionService.Message;
                        return RedirectToAction("Login");

                    }
                    else
                    {
                        string res = await endpoint.Content.ReadAsStringAsync();
                        var resul = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(res);
                        ViewBag.Error = resul.Message;
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
                        if (endpoint.IsSuccessStatusCode)
                        {
                            string response = await endpoint.Content.ReadAsStringAsync();
                            var sesionService = JsonConvert.DeserializeObject<ServicesResultModel<CheckSesionModel>>(response);
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
                        string res = await endpoint.Content.ReadAsStringAsync();
                        var resul = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(res);
                        TempData["Error"] = resul.Message;
                        return RedirectToAction("Login");
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
                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var sesionService = JsonConvert.DeserializeObject<ServicesResultModel<CheckSesionModel>>(response);
                        if (sesionService != null && sesionService.Success && sesionService.Data != null)
                        {
                            active = sesionService.Data.Estado; // true = activa, false = inactiva
                            return Ok(new { active });
                        }
                        else
                        {
                            return Ok(new { active });
                        }
                    }
                    else
                    {
                        string res = await endpoint.Content.ReadAsStringAsync();
                        var resul = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(res);
                        return View("Error", resul.Message);
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

                    if (endpoint.IsSuccessStatusCode)
                    {
                        var response = await endpoint.Content.ReadAsStringAsync();
                        var sesionService = JsonConvert.DeserializeObject<ServicesResultModel<CheckSesionModel>>(response);
                        if (sesionService != null && sesionService.Success && sesionService.Data != null)
                        {
                            active = sesionService.Data.Estado;
                            return Ok(new { active });// true = activa, false = inactiva
                        }else
                            return Ok(new { active = false });


                    }
                    string res = await endpoint.Content.ReadAsStringAsync();
                    var resul = JsonConvert.DeserializeObject<ServicesResultModel<SesionModel>>(res);
                    TempData["Error"] = resul.Message;
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
