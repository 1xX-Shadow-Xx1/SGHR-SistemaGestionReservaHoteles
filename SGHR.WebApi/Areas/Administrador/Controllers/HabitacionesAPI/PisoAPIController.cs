using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Piso;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    [Area("Administrador")]
    public class PisoAPIController : Controller
    {

        public PisoAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar pisos ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointPiso = await httpclient.GetAsync($"Piso/Get-Piso-ByID?id={id}");

                        if (endpointPiso.IsSuccessStatusCode)
                        {
                            string response = await endpointPiso.Content.ReadAsStringAsync();
                            var resultPiso = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);

                            if (resultPiso != null && resultPiso.Success)
                            {
                                TempData["Success"] = resultPiso.Message;
                                return PartialView("_List", new List<PisoModel> { resultPiso.Data });
                            }
                            else
                            {
                                TempData["Error"] = resultPiso.Message;
                                return PartialView("_List", new List<PisoModel>());
                            }

                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointPiso.StatusCode}";
                            return PartialView("_List", new List<PisoModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("Piso/Get-Pisos");

                        if (endpointList.IsSuccessStatusCode)
                        {
                            string responseList = await endpointList.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<PisoModel>>>(responseList);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<PisoModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointList.StatusCode}";
                            return PartialView("_List", new List<PisoModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los pisos.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- Vista completa de detalles de Piso ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Piso/Get-Piso-ByID?id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);

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
                ViewBag.Error = "Ocurrió un error interno al obtener los pisos.";
                return View("Error", ex.Message);
            }
        }

        // GET: Crear Piso
        public IActionResult Create()
        {
            var model = new CreatePisoModel();
            return View(model);
        }

        // POST: Crear Piso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePisoModel model)
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
                    var endpointCreate = await httpclient.PostAsJsonAsync("Piso/Create-Piso", model);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var resultPiso = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);

                        if (resultPiso != null && resultPiso.Success)
                        {
                            TempData["Success"] = resultPiso.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultPiso.Message;
                            return View(model);
                        }

                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointCreate.StatusCode}";
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear el piso.";
                return View("Error", ex.Message);
            }
        }

        // GET: Edit Piso
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Piso/Get-Piso-ByID?id={id}");

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string responseEdit = await endpointEdit.Content.ReadAsStringAsync();
                        var resultPiso = JsonConvert.DeserializeObject<ServicesResultModel<UpdatePisoModel>>(responseEdit);

                        if (resultPiso != null && resultPiso.Success)
                        {
                            TempData["Success"] = resultPiso.Message;
                            return View(resultPiso.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultPiso.Message;
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
                ViewBag.Error = "Ocurrió un error interno al mostrar la vista de editar.";
                return View("Error", ex.Message);
            }
        }

        // POST: Edit Piso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePisoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Piso/Update-Piso", model);

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var ResultPiso = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);

                        if (ResultPiso != null && ResultPiso.Success)
                        {
                            TempData["Success"] = ResultPiso.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = ResultPiso.Message;
                            return View(model);
                        }

                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
                        return View(model);
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el piso.";
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
                    var endpoint = await httpclient.GetAsync($"Piso/Get-Piso-ByID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultPiso = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);

                        if (resultPiso != null && resultPiso.Success)
                        {
                            TempData["Success"] = resultPiso.Message;
                            return PartialView("_Delete", resultPiso.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultPiso.Message;
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
                ViewBag.Error = "Ocurrió un error interno al eliminar el piso.";
                return View("Error", ex.Message);
            }
        }

        // POST: Delete Confirmed
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Piso/Remove-Piso?id={id}", null);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<PisoModel>>(response);
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el piso.";
                return View("Error", ex.Message);
            }
        }
















        /*private readonly IPisoServices _pisoServices;

        public PisoAPIController(IPisoServices pisoServices)
        {
            _pisoServices = pisoServices;
        }

        // GET: PisoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PisoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _pisoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var piso = result.Data as PisoDto;
            return View(piso); // Vista completa
        }

        //GET: Partial para listar pisos
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _pisoServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<PisoDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<PisoDto> { (PisoDto)result.Data });
            }
            else
            {
                var result = await _pisoServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaPisos = result.Data as IEnumerable<PisoDto>;
                return PartialView("_List", listaPisos);
            }
        }

        // GET: PisoController/Create
        public IActionResult Create()
        {
            var model = new CreatePisoDto();
            return View(model); // Vista completa
        }

        // POST: PisoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePisoDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _pisoServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de pisos o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: PisoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _pisoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdatePisoDto piso = new UpdatePisoDto
            {
                Id = result.Data.Id,
                NumeroPiso = result.Data.NumeroPiso,
                Descripcion = result.Data.Descripcion,
                Estado = result.Data.Estado
            };
            return View(piso); // Vista completa
        }

        // POST: PisoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatePisoDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _pisoServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: PisoController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _pisoServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            return PartialView("_Delete", (PisoDto)result.Data);

        }

        // POST: PisoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _pisoServices.DeleteAsync(id);
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
