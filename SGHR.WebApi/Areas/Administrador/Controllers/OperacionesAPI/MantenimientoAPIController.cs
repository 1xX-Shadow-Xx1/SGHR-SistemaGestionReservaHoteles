using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.OperacionesAPI
{
    [Area("Administrador")]
    public class MantenimientoAPIController : Controller
    {
        public MantenimientoAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar mantenimientos ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointMant = await httpclient.GetAsync($"Mantenimiento/Get-By-Id?id={id}");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointMant.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointMant.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultMant = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpointMant);

                        if (resultMant != null && resultMant.Success)
                        {
                            TempData["Success"] = resultMant.Message;
                            return PartialView("_List", new List<MantenimientoModel> { resultMant.Data });
                        }
                        else
                        {
                            TempData["Error"] = resultMant.Message;
                            return PartialView("_List", new List<MantenimientoModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("Mantenimiento/Get-All");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointList.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointList.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultList = await new JsonConvertidor<MantenimientoModel>().DeserializarList(endpointList);

                        if (resultList != null && resultList.Success)
                        {
                            TempData["Success"] = resultList.Message;
                            return PartialView("_List", resultList.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultList.Message;
                            return PartialView("_List", new List<MantenimientoModel>());
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los mantenimientos.";
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
                    var endpointDetail = await httpclient.GetAsync($"Mantenimiento/Get-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpointDetail);

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
                ViewBag.Error = "Ocurrió un error interno al obtener el mantenimiento.";
                return View("Error", ex.Message);
            }
        }

        // GET: vista completa de creación de mantenimiento
        public IActionResult Create()
        {
            var model = new CreateMantenimientoModel();
            return View(model);
        }

        // POST: creación de mantenimiento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMantenimientoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Mantenimiento/Create-Mantenimiento", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointCreate.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointCreate.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultMant = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpointCreate);

                    if (resultMant != null && resultMant.Success)
                    {
                        TempData["Success"] = resultMant.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = resultMant.Message;
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear el mantenimiento.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar mantenimiento
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Mantenimiento/Get-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultMant = await new JsonConvertidor<UpdateMantenimientoModel>().Deserializar(endpointEdit);

                    if (resultMant != null && resultMant.Success)
                    {
                        TempData["Success"] = resultMant.Message;
                        return View(resultMant.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultMant.Message;
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

        // POST: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMantenimientoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Mantenimiento/Update-Mantenimiento", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultMant = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpointEdit);

                    if (resultMant != null && resultMant.Success)
                    {
                        TempData["Success"] = resultMant.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = resultMant.Message;
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el mantenimiento.";
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
                    var endpoint = await httpclient.GetAsync($"Mantenimiento/Get-By-Id?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultMant = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpoint);

                    if (resultMant != null && resultMant.Success)
                    {
                        TempData["Success"] = resultMant.Message;
                        return PartialView("_Delete", resultMant.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultMant.Message;
                        return RedirectToAction("Index");
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al eliminar el mantenimiento.";
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
                    var endpointRemove = await httpclient.PutAsync($"Mantenimiento/Remove-Mantenimiento?id={id}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointRemove.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointRemove.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<MantenimientoModel>().Deserializar(endpointRemove);

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
                ViewBag.Error = "Error interno al eliminar el mantenimiento.";
                return View("Error", ex.Message);
            }
        }










        /*private readonly IMantenimientoServices _mantenimientoServices;

        public MantenimientoAPIController(IMantenimientoServices mantenimientoServices)
        {
            _mantenimientoServices = mantenimientoServices;
        }

        // GET: MantenimientoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MantenimientoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _mantenimientoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var mantenimiento = result.Data as MantenimientoDto;
            return View(mantenimiento); // Vista completa
        }

        //GET: Partial para listar reservas
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _mantenimientoServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                   
                    return PartialView("_List", new List<MantenimientoDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<MantenimientoDto> { (MantenimientoDto)result.Data });
            }
            else
            {
                var result = await _mantenimientoServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }         

                var listaMantenimientos = result.Data as IEnumerable<MantenimientoDto>;
                return PartialView("_List", listaMantenimientos);
            }
        }

        // GET: MantenimientoController/Create
        public IActionResult Create()
        {
            var model = new CreateMantenimientoDto();
            return View(model); // Vista completa
        }

        // POST: MantenimientoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMantenimientoDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _mantenimientoServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
  
            }

            // Redirigir a la lista de mantenimiento o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: MantenimientoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mantenimientoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
                  // o mostrar una página de error
            UpdateMantenimientoDto mantenimiento = new UpdateMantenimientoDto
            {
                Id = result.Data.Id,
                Descripcion = result.Data.Descripcion,
                FechaInicio = result.Data.FechaInicio,
                FechaFin = result.Data.FechaFin,
                Estado = result.Data.Estado,
                NumeroHabitacion = result.Data.NumeroHabitacion,
                NumeroPiso = result.Data.NumeroPiso,
                RealizadoPor = result.Data.RealizadoPor
            };
            return View(mantenimiento); // Vista completa
        }

        // POST: MantenimientoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMantenimientoDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _mantenimientoServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: MantenimientoController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _mantenimientoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
                

            if (result.Data == null)
            {
                TempData["Error"] = "No se encontró el mantenimiento.";
                return PartialView("_Error");
            }               

            return PartialView("_Delete", (MantenimientoDto)result.Data);

        }

        // POST: MantenimientoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _mantenimientoServices.DeleteAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, data = result.Data, message = result.Message });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, data = result.Data, message = result.Message });
        }*/
    }
}
