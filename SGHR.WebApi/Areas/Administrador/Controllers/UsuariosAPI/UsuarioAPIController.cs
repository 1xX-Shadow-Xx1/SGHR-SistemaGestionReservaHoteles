using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Sesion;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.UsuariosAPI
{
    [Area("Administrador")]
    public class UsuarioAPIController : Controller
    {

        public UsuarioAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar usuarios ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointUser = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id?id={id}");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointUser.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointUser.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resulUser = await new JsonConvertidor<UsuarioModel>().Deserializar(endpointUser);

                        if (resulUser != null && resulUser.Success)
                        {
                            TempData["Success"] = resulUser.Message;
                            return PartialView("_List", new List<UsuarioModel> { resulUser.Data });
                        }
                        else
                        {
                            TempData["Error"] = resulUser.Message;
                            return PartialView("_List", new List<UsuarioModel>());
                        }
                    }
                    else
                    {
                        var endpointlista = await httpclient.GetAsync("Usuario/Get-Usuarios");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointlista.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointlista.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resulList = await new JsonConvertidor<UsuarioModel>().DeserializarList(endpointlista);

                        if (resulList != null && resulList.Success)
                        {
                            TempData["Success"] = resulList.Message;
                            return PartialView("_List", resulList.Data);
                        }
                        else
                        {
                            TempData["Error"] = resulList.Message;
                            return PartialView("_List", new List<UsuarioModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error interno al obtener los usuarios.";
                return PartialView("Error", ex.Message);
            }
        }
            
        // --- Vista completa de detalles del usuario ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<UsuarioModel>().Deserializar(endpointDetail);

                    if (resultDetail != null && resultDetail.Success)
                    {
                        TempData["Success"] = resultDetail.Message;
                        return View(resultDetail.Data);

                    }
                    else
                    {
                        TempData["Error"] = resultDetail.Message;
                        return RedirectToAction("Index");
                    }

                }


            }catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error interno al obtener los usuarios.";
                return View("Error", ex.Message);
            }
        }


        // GET: vista completa de creación de usuario
        public IActionResult Create()
        {
            var model = new CreateUsuarioModel();
            return View(model); // Vista completa, no partial
        }

        // POST: creación de usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUsuarioModel dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointcreate = await httpclient.PostAsJsonAsync("Usuario/create-Usuario", dto);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointcreate.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointcreate.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultUser = await new JsonConvertidor<UsuarioModel>().Deserializar(endpointcreate);

                    if (resultUser != null && resultUser.Success)
                    {
                        TempData["Success"] = resultUser.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = resultUser.Message;
                        return View(dto);
                    }
                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error interno al crear el usuario.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar usuario (vista completa)
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultUser = await new JsonConvertidor<UpdateUsuarioModel>().Deserializar(endpointEdit);

                    if (resultUser != null && resultUser.Success)
                    {
                        TempData["Success"] = resultUser.Message;
                        return View(resultUser.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultUser.Message;
                        return RedirectToAction("Index");
                    }
                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Error interno al mostrar la vista de editar.";
                return View("Error", ex.Message);
            }
        }

        // POST: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUsuarioModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                using(var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Usuario/update-Usuario", dto);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var ResultUser = await new JsonConvertidor<UsuarioModel>().Deserializar(endpointEdit);

                    if (ResultUser != null && ResultUser.Success)
                    {
                        TempData["Success"] = ResultUser.Message;
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        TempData["Error"] = ResultUser.Message;
                        return View(dto);
                    }
                }

            } catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el usuario.";
                return View("Error", ex.Message);
            }
        }


        // --- Partial para eliminar ---
        public async Task<IActionResult> _Delete(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultUser = await new JsonConvertidor<UsuarioModel>().Deserializar(endpoint);

                    if (resultUser != null && resultUser.Success)
                    {
                        TempData["Success"] = resultUser.Message;
                        return PartialView("_Delete", resultUser.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultUser.Message;
                        return RedirectToAction("Index");
                    }
                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Ocurrio un error interno al eliminar el usuario.";
                return View("Error", ex.Message);
            }
        }

        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Usuario/Remove-Usuario?id={id}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointRemove.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointRemove.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<UsuarioModel>().Deserializar(endpointRemove);

                    return Json(new { success = true, message = result.Message, data = result.Data });

                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el usuario.";
                return View("Error", ex.Message);
            }
        }
    }
}
