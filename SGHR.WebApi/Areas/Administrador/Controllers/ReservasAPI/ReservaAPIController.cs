
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Models.Reservas.ServicioAdicional;

namespace SGHR.Web.Areas.Administrador.Controllers.ReservasAPI
{
    [Area("Administrador")]
    public class ReservaAPIController : Controller
    {
        public ReservaAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar reservas ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointReserva = await httpclient.GetAsync($"Reserva/Get-Reserva-ByID?id={id}");

                        if (endpointReserva.IsSuccessStatusCode)
                        {
                            string response = await endpointReserva.Content.ReadAsStringAsync();
                            var resultReserva = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);

                            if (resultReserva != null && resultReserva.Success)
                            {
                                TempData["Success"] = resultReserva.Message;
                                return PartialView("_List", new List<ReservaModel> { resultReserva.Data });
                            }
                            else
                            {
                                TempData["Error"] = resultReserva.Message;
                                return PartialView("_List", new List<ReservaModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointReserva.StatusCode}";
                            return PartialView("_List", new List<ReservaModel>());
                        }
                    }
                    else
                    {
                        var endpointLista = await httpclient.GetAsync("Reserva/Get-Reservas");

                        if (endpointLista.IsSuccessStatusCode)
                        {
                            string responseList = await endpointLista.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<ReservaModel>>>(responseList);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<ReservaModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointLista.StatusCode}";
                            return PartialView("_List", new List<ReservaModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener las reservas.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- Vista completa de detalles de reserva ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Reserva/Get-Reserva-ByID?id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);

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
                ViewBag.Error = "Ocurrió un error interno al obtener la reserva.";
                return View("Error", ex.Message);
            }
        }

        // GET: Crear reserva
        public IActionResult Create()
        {
            var model = new CreateReservaModel();
            return View(model);
        }

        // POST: Crear reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Reserva/Create-Reserva", model);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var resultReserva = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);

                        if (resultReserva != null && resultReserva.Success)
                        {
                            TempData["Success"] = resultReserva.Message;
                            return RedirectToAction("Servicios", new { id = resultReserva.Data.Id });
                        }
                        else
                        {
                            TempData["Error"] = resultReserva.Message;
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
                ViewBag.Error = "Ocurrió un error interno al crear la reserva.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar reserva
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Reserva/Get-Reserva-ByID?id={id}");

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var resultReserva = JsonConvert.DeserializeObject<ServicesResultModel<UpdateReservaModel>>(response);

                        if (resultReserva != null && resultReserva.Success)
                        {
                            TempData["Success"] = resultReserva.Message;
                            return View(resultReserva.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultReserva.Message;
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

        // POST: Editar reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReservaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Reserva/Update-Reserva", model);

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var resultReserva = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);

                        if (resultReserva != null && resultReserva.Success)
                        {
                            TempData["Success"] = resultReserva.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultReserva.Message;
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
                ViewBag.Error = "Error interno al actualizar la reserva.";
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
                    var endpoint = await httpclient.GetAsync($"Reserva/Get-Reserva-ByID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultReserva = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);

                        if (resultReserva != null && resultReserva.Success)
                        {
                            TempData["Success"] = resultReserva.Message;
                            return PartialView("_Delete", resultReserva.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultReserva.Message;
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
                ViewBag.Error = "Ocurrió un error interno al eliminar la reserva.";
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
                    var endpointRemove = await httpclient.PutAsync($"Reserva/Remove-Reserva?id={id}", null);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar la reserva.";
                return View("Error", ex.Message);
            }
        }

        // GET: Servicios de una reserva
        [HttpGet]
        public IActionResult Servicios(int id)
        {
            ViewBag.IdReserva = id;
            return View("Servicios");
        }

        // GET: Servicios por reserva
        [HttpGet]
        public async Task<IActionResult> GetServiciosPorReserva(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Reserva/Get-Servicios-By-ReservaID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<List<ServicioAdicionalModel>>>(response);
                        TempData["Success"] = result.Message;
                        return Json(new { success = true, data = result.Data });
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return Json(new { success = false, message = $"Error {endpoint.StatusCode}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al obtener los servicios de la reserva.";
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Servicios disponibles
        [HttpGet]
        public async Task<IActionResult> GetServiciosDisponibles()
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync("ServicioAdicional/Get-Servicio-Adicional");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<List<ServicioAdicionalModel>>>(response);
                        TempData["Success"] = result.Message;
                        return Json(new { success = true, data = result.Data });
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return Json(new { success = false, message = $"Error {endpoint.StatusCode}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al obtener los servicios disponibles.";
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Agregar servicio a reserva
        [HttpPost]
        public async Task<IActionResult> AgregarServicio(int idReserva, string nombreServicio)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Reserva/Add-Servicio-Adicional-to-Reserva?id={idReserva}&nameServicio={nombreServicio}", null);

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        TempData["Success"] = result.Message;
                        return Json(new { success = true, message = result.Message });
                    }
                    else
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        TempData["Error"] = result.Message;
                        return Json(new { success = false, message = result.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al agregar el servicio a la reserva.";
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Remover servicio de reserva
        [HttpPost]
        public async Task<IActionResult> RemoverServicio(int idReserva, string nombreServicio)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsync($"Reserva/Remove-Servicio-Adicional-to-Reserva?id={idReserva}&nombreServicio={nombreServicio}", null);

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        TempData["Success"] = result.Message;
                        return Json(new { success = true, message = result.Message });
                    }
                    else
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<ReservaModel>>(response);
                        TempData["Error"] = result.Message;
                        return Json(new { success = false, message = result.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al remover el servicio de la reserva.";
                return Json(new { success = false, message = ex.Message });
            }
        }























        /*private readonly IReservaServices _reservaServices;
        private readonly IServicioAdicionalServices _servicioAdicionalServices;

        public ReservaAPIController(IReservaServices reservaServices,
                                 IServicioAdicionalServices servicioAdicionalServices)
        {
            _reservaServices = reservaServices;
            _servicioAdicionalServices = servicioAdicionalServices;
        }

        // GET: ReservaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var reserva = result.Data as ReservaDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar reservas
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _reservaServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ReservaDto>()); // lista vacía si no se encuentra
                }
                return PartialView("_List", new List<ReservaDto> { (ReservaDto)result.Data });
            }
            else
            {
                var result = await _reservaServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaReservas = result.Data as IEnumerable<ReservaDto>;
                return PartialView("_List", listaReservas);
            }
        }

        // GET: ReservaController/Create
        public IActionResult Create()
        {
            var model = new CreateReservaDto();
            return View(model); // Vista completa
        }

        // POST: ReservaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservaDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _reservaServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de habitaciones o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Servicios", new { id = result.Data.Id });
        }

        // GET: ReservaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateReservaDto habitacion = new UpdateReservaDto
            {
                Id = result.Data.Id,
                NumeroHabitacion = result.Data.NumeroHabitacion,
                CedulaCliente = result.Data.CedulaCliente,
                CorreoCliente = result.Data.CorreoCliente,
                FechaInicio = result.Data.FechaInicio,
                FechaFin = result.Data.FechaFin,
                Estado = result.Data.Estado
            };
            return View(habitacion); // Vista completa
        }

        // POST: ReservaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReservaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _reservaServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ReservaController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return PartialView("_Error");
            }

            return PartialView("_Delete", (ReservaDto)result.Data);

        }

        // POST: ReservaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _reservaServices.DeleteAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message, data = result.Data });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, message = result.Message, data = result.Data });
        }

        // GET: ReservaController/ViewServicios/5
        [HttpGet]
        public IActionResult Servicios(int id)
        {
            ViewBag.IdReserva = id;
            return View("Servicios");
        }

        // GET: ReservaController/ServiciosPorReservas/5
        [HttpGet]
        public async Task<IActionResult> GetServiciosPorReserva(int id)
        {
            var result = await _reservaServices.GetServiciosByReservaId(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, data = result.Data });
        }
        // GET: ReservaController/ServiciosDisponibles/5
        [HttpGet]
        public async Task<IActionResult> GetServiciosDisponibles()
        {
            var result = await _servicioAdicionalServices.GetAllAsync();

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, data = result.Data });
        }

        // POST: ReservaController/AgregarServicio/5
        [HttpPost]
        public async Task<IActionResult> AgregarServicio(int idReserva, string nombreServicio)
        {
            var result = await _reservaServices.AddServicioAdicional(idReserva, nombreServicio);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, message = result.Message });
        }

        // POST: ReservaController/RemoverServicio/5
        [HttpPost]
        public async Task<IActionResult> RemoverServicio(int idReserva, string nombreServicio)
        {
            var result = await _reservaServices.RemoveServicioAdicional(idReserva, nombreServicio);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new { success = false, message = result.Message });
            }
            TempData["Success"] = result.Message;
            return Json(new { success = true, message = result.Message });
        }*/


    }
}
