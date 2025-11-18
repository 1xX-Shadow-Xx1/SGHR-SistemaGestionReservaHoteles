using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Reporte;

namespace SGHR.Web.Areas.Administrador.Controllers.OperacionesAPI
{
    [Area("Administrador")]
    public class ReporteAPIController : Controller
    {
        public ReporteAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar reportes ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointReporte = await httpclient.GetAsync($"Reporte/get-Reporte-ById?id={id}");

                        if (endpointReporte.IsSuccessStatusCode)
                        {
                            string response = await endpointReporte.Content.ReadAsStringAsync();
                            var resultReporte = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);

                            if (resultReporte != null && resultReporte.Success)
                            {
                                TempData["Success"] = resultReporte.Message;
                                return PartialView("_List", new List<ReporteModel> { resultReporte.Data });
                            }
                            else
                            {
                                TempData["Error"] = resultReporte.Message;
                                return PartialView("_List", new List<ReporteModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointReporte.StatusCode}";
                            return PartialView("_List", new List<ReporteModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("Reporte/get-Reportes");

                        if (endpointList.IsSuccessStatusCode)
                        {
                            string response = await endpointList.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<ReporteModel>>>(response);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<ReporteModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointList.StatusCode}";
                            return PartialView("_List", new List<ReporteModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los reportes.";
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
                    var endpointDetail = await httpclient.GetAsync($"Reporte/get-Reporte-ById?id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);

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
                ViewBag.Error = "Ocurrió un error interno al obtener los reportes.";
                return View("Error", ex.Message);
            }
        }

        // GET: Crear reporte
        public IActionResult Create()
        {
            var model = new CreateReporteModel();
            return View(model);
        }

        // POST: Crear reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReporteModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Reporte/create-Reporte", model);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpointCreate.StatusCode}";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear el reporte.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar reporte
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Reporte/get-Reporte-ById?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<UpdateReporteModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al cargar la vista de edición.";
                return View("Error", ex.Message);
            }
        }

        // POST: Editar reporte
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReporteModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsJsonAsync("Reporte/update-Reporte", model);

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el reporte.";
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
                    var endpoint = await httpclient.GetAsync($"Reporte/get-Reporte-ById?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el reporte.";
                return View("Error", ex.Message);
            }
        }

        // POST: Confirmación del eliminado
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Reporte/Remove-Reporte?id={id}", null);

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReporteModel>>(response);
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el reporte.";
                return View("Error", ex.Message);
            }
        }













        /*private readonly IReporteServices _reporteServices;

        public ReporteAPIController(IReporteServices reporteServices)
        {
            _reporteServices = reporteServices;
        }

        // GET: ReporteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReporteController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _reporteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }
            var reserva = result.Data as ReporteDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar reportes
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _reporteServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ReporteDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<ReporteDto> { (ReporteDto)result.Data });
            }
            else
            {
                var result = await _reporteServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaReportes = result.Data as IEnumerable<ReporteDto>;
                return PartialView("_List", listaReportes);
            }
        }

        // GET: ReporteController/Create
        public IActionResult Create()
        {
            var model = new CreateReporteDto();
            return View(model); // Vista completa
        }

        // POST: ReporteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReporteDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _reporteServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de reportes o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ReporteController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _reporteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateReporteDto reporte = new UpdateReporteDto
            {
                Id = result.Data.Id,
                GeneradoPor = result.Data.GeneradoPor,
                RutaArchivo = result.Data.RutaArchivo,
                TipoReporte = result.Data.TipoReporte
            };
            return View(reporte); // Vista completa
        }

        // POST: ReporteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReporteDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _reporteServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ReporteController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _reporteServices.GetByIdAsync(id);
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
                

            return PartialView("_Delete", (ReporteDto)result.Data);

        }

        // POST: ReporteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _reporteServices.DeleteAsync(id);
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
