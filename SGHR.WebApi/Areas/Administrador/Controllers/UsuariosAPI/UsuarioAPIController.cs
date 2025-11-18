using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Usuario;

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

                        var endpointUser = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id={id}");

                        if (endpointUser.IsSuccessStatusCode)
                        {
                            string response = await endpointUser.Content.ReadAsStringAsync();
                            var resulUser = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);

                            if (resulUser != null && resulUser.Success)
                            {
                                TempData["Success"] = resulUser.Message;
                                return PartialView("_List", resulUser.Data);
                            }
                            else
                            {
                                TempData["Error"] = resulUser.Message;
                                return PartialView("_List", new List<UsuarioModel>());
                            }

                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointUser.StatusCode}";
                            return PartialView("_List", new List<UsuarioModel>());
                        }
                    }
                    else
                    {
                        var endpointlista = await httpclient.GetAsync("Usuario/Get-Usuarios");

                        if (endpointlista.IsSuccessStatusCode)
                        {
                            string responseList = await endpointlista.Content.ReadAsStringAsync();
                            var resulList = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(responseList);

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
                        else
                        {
                            TempData["Error"] = $"Error {endpointlista.StatusCode}";
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
                using(var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpointDetail.StatusCode}";
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
                using(var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointcreate = await httpclient.PostAsJsonAsync("Usuario/create-Usuario", dto);

                    if (endpointcreate.IsSuccessStatusCode)
                    {
                        string response = await endpointcreate.Content.ReadAsStringAsync();
                        var resultUser = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpointcreate.StatusCode}";
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
                using( var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id={id}");
                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string responseEdit = await endpointEdit.Content.ReadAsStringAsync();
                        var resultUser = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(responseEdit);

                        if(resultUser != null && resultUser.Success)
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
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
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
        public async Task<IActionResult> Edit(UpdateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                using(var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Usuario/update-Usuario", dto);

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var ResultUser = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
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
                    var endpoint = await httpclient.GetAsync($"Usuario/Get-Usuario-By-Id={id}");

                    if(endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultUser = JsonConvert.DeserializeObject<ServicesResultModel<UsuarioModel>>(response);

                        if(resultUser != null && resultUser.Success)
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
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
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
                    var endpointRemove = await httpclient.PutAsJsonAsync($"Usuario/Remove-Usuario?id", id);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        return Json(response);
                    }
                    else
                    {
                        return Json(new { success = false, message = $"Error {endpointRemove.StatusCode}"});
                    }
                }

            }catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el usuario.";
                return View("Error", ex.Message);
            }
        }
    }
}
