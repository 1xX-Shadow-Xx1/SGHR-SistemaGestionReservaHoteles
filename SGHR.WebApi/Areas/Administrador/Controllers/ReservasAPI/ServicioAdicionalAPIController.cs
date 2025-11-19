using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.ServicioAdicional;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.ReservasAPI
{
    [Area("Administrador")]
    public class ServicioAdicionalAPIController : Controller
    {
        public ServicioAdicionalAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Vista completa de detalles del servicio adicional ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"ServicioAdicional/Get-Servicio-Adicional-By-ID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpointDetail);

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
                ViewBag.Error = "Ocurrió un error interno al obtener el servicio adicional.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial para listar servicios adicionales ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpoint = await httpclient.GetAsync($"ServicioAdicional/Get-Servicio-Adicional-By-ID?id={id}");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                        }

                        var result = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpoint);

                        if (result != null && result.Success)
                        {
                            TempData["Success"] = result.Message;
                            return PartialView("_List", new List<ServicioAdicionalModel> { result.Data });
                        }
                        else
                        {
                            TempData["Error"] = result.Message;
                            return PartialView("_List", new List<ServicioAdicionalModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("ServicioAdicional/Get-Servicio-Adicional");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointList.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointList.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultList = await new JsonConvertidor<ServicioAdicionalModel>().DeserializarList(endpointList);

                        if (resultList != null && resultList.Success)
                        {
                            TempData["Success"] = resultList.Message;
                            return PartialView("_List", resultList.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultList.Message;
                            return PartialView("_List", new List<ServicioAdicionalModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener la lista.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- Vista completa para crear un servicio adicional ---
        public IActionResult Create()
        {
            var model = new CreateServicioAdicionalModel();
            return View(model);
        }

        // --- POST crear servicio adicional ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServicioAdicionalModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("ServicioAdicional/Create-Servicio-Adicional", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointCreate.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointCreate.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpointCreate);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear el servicio adicional.";
                return View("Error", ex.Message);
            }
        }

        // --- Vista completa editar servicio adicional ---
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"ServicioAdicional/Get-Servicio-Adicional-By-ID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<UpdateServicioAdicionalModel>().Deserializar(endpoint);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return View(result.Data);
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al mostrar la vista de edición.";
                return View("Error", ex.Message);
            }
        }

        // --- POST actualizar servicio adicional ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateServicioAdicionalModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsJsonAsync("ServicioAdicional/Update-Servicio-Adicional", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpoint);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el servicio adicional.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial eliminar ---
        public async Task<IActionResult> _Delete(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"ServicioAdicional/Get-Servicio-Adicional-By-ID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpoint);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_Delete", result.Data);
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al eliminar el servicio adicional.";
                return View("Error", ex.Message);
            }
        }

        // --- POST eliminar registro ---
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"ServicioAdicional/Remove-Servicio-Adicional?id={id}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<ServicioAdicionalModel>().Deserializar(endpoint);

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
                ViewBag.Error = "Error interno al eliminar el servicio adicional.";
                return View("Error", ex.Message);
            }
        }














        /*private readonly IServicioAdicionalServices _servicioAdicionalServices;

        public ServicioAdicionalAPIController(IServicioAdicionalServices servicioAdicionalServices)
        {
            _servicioAdicionalServices = servicioAdicionalServices;
        }

        // GET: ServicioAdicionalController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServicioAdicionalController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var reserva = result.Data as ServicioAdicionalDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar servicios adicionales
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _servicioAdicionalServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ServicioAdicionalDto>()); // lista vacía si no se encuentra
                }
                return PartialView("_List", new List<ServicioAdicionalDto> { (ServicioAdicionalDto)result.Data });
            }
            else
            {
                var result = await _servicioAdicionalServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaServicios = result.Data as IEnumerable<ServicioAdicionalDto>;
                return PartialView("_List", listaServicios);
            }
        }

        // GET: ServicioAdicionalController/Create
        public IActionResult Create()
        {
            var model = new CreateServicioAdicionalDto();
            return View(model); // Vista completa
        }

        // POST: ServicioAdicionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServicioAdicionalDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _servicioAdicionalServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de habitaciones o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ServicioAdicionalController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateServicioAdicionalDto servicio = new UpdateServicioAdicionalDto
            {
                Id = result.Data.Id,
                Nombre = result.Data.Nombre,
                Descripcion = result.Data.Descripcion,
                Precio = result.Data.Precio,
                Estado = result.Data.Estado
            };
            return View(servicio); // Vista completa
        }

        // POST: ServicioAdicionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateServicioAdicionalDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _servicioAdicionalServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ServicioAdicionalController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Servicio no encontrado.";
                return PartialView("_Error");
            }

            return PartialView("_Delete", (ServicioAdicionalDto)result.Data);

        }

        // POST: ServicioAdicionalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _servicioAdicionalServices.DeleteAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message, data = result.Data });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, message = result.Message, data = result.Data });
        }*/
    }
}
