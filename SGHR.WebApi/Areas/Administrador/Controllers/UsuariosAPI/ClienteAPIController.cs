using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Validador;

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

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointClient.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointClient.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultClient = await new JsonConvertidor<ClienteModel>().Deserializar(endpointClient);

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
                        var endpointClients = await httpclient.GetAsync("Cliente/Get-Clientes");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointClients.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointClients.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultList = await new JsonConvertidor<ClienteModel>().DeserializarList(endpointClients);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<ClienteModel>().Deserializar(endpointDetail);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointCreate.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointCreate.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultClient = await new JsonConvertidor<ClienteModel>().Deserializar(endpointCreate);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultClient = await new JsonConvertidor<UpdateClienteModel>().Deserializar(endpointEdit);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultClient = await new JsonConvertidor<ClienteModel>().Deserializar(endpointEdit);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultClient = await new JsonConvertidor<ClienteModel>().Deserializar(endpoint);

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

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointRemove.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointRemove.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<ClienteModel>().Deserializar(endpointRemove);

                    if (result != null && result.Success)
                    {
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
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
