using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;

namespace SGHR.Web.Areas.Administrador.Controllers.UsuariosAPI
{
    [Area("Administrador")]
    public class ClienteAPIController : Controller
    {
        public ClienteAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar clientes ---
        public async Task<IActionResult> _List(string? cedula)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (!string.IsNullOrEmpty(cedula))
                    {
                        var endpointClient = await httpclient.GetAsync($"Cliente/Get-Cliente-by-cedula?cedula={cedula}");

                        if (endpointClient.IsSuccessStatusCode)
                        {
                            string response = await endpointClient.Content.ReadAsStringAsync();
                            var resultClient = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);

                            if (resultClient != null && resultClient.Success)
                            {
                                TempData["Success"] = resultClient.Message;
                                return PartialView("_List", new List<ClienteModel> { resultClient.Data });
                            }
                            else
                            {
                                TempData["Error"] = resultClient.Message;
                                return PartialView("_List", new List<ClienteModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointClient.StatusCode}";
                            return PartialView("_List", new List<ClienteModel>());
                        }
                    }
                    else
                    {
                        var endpointClients = await httpclient.GetAsync("Cliente/Get-Clientes");

                        if (endpointClients.IsSuccessStatusCode)
                        {
                            string responseList = await endpointClients.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<ClienteModel>>>(responseList);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<ClienteModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointClients.StatusCode}";
                            return PartialView("_List", new List<ClienteModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los clientes.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- Vista completa de detalles ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Cliente/Get-Cliente-ByID?id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);

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
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener el cliente.";
                return View("Error", ex.Message);
            }
        }

        // GET: Crear cliente
        public IActionResult Create()
        {
            var model = new CreateClienteModel();
            return View(model);
        }

        // POST: Crear cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClienteModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Cliente/Create-Cliente", dto);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var resultClient = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);

                        if (resultClient != null && resultClient.Success)
                        {
                            TempData["Success"] = resultClient.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultClient.Message;
                            return View(dto);
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointCreate.StatusCode}";
                        return View(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear el cliente.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar cliente
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Cliente/Get-Cliente-ByID?id={id}");

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string responseEdit = await endpointEdit.Content.ReadAsStringAsync();
                        var resultClient = JsonConvert.DeserializeObject<ServicesResultModel<UpdateClienteModel>>(responseEdit);

                        if (resultClient != null && resultClient.Success)
                        {
                            TempData["Success"] = resultClient.Message;
                            return View(resultClient.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultClient.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al mostrar la vista de editar.";
                return View("Error", ex.Message);
            }
        }

        // POST: Editar cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateClienteModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Cliente/Update-Cliente", dto);

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var resultClient = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);

                        if (resultClient != null && resultClient.Success)
                        {
                            TempData["Success"] = resultClient.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultClient.Message;
                            return View(dto);
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
                        return View(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el cliente.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial Delete ---
        public async Task<IActionResult> _Delete(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Cliente/Get-Cliente-ByID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultClient = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);

                        if (resultClient != null && resultClient.Success)
                        {
                            TempData["Success"] = resultClient.Message;
                            return PartialView("_Delete", resultClient.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultClient.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al eliminar el cliente.";
                return View("Error", ex.Message);
            }
        }

        // POST: Confirm Delete
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Cliente/Remove-Cliente?id={id}", null);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ClienteModel>>(response);
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el cliente.";
                return View("Error", ex.Message);
            }
        }


    }
}
