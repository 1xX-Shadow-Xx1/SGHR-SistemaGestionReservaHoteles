using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    [Area("Administrador")]
    public class HabitacionAPIController : Controller
    {
        public HabitacionAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Vista completa de detalles de la habitación ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Habitacion/Get-Habitacion-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<HabitacionModel>().Deserializar(endpointDetail);

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
                ViewBag.Error = "Ocurrió un error interno al obtener la habitación.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial para listar habitaciones ---
        public async Task<IActionResult> _List(string? numeroHabitacion)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (!string.IsNullOrEmpty(numeroHabitacion))
                    {
                        var endpoint = await httpclient.GetAsync($"Habitacion/Get-Habitacion-By-NumeroHabitacion?numeroHabitacion={numeroHabitacion}");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultRoom = await new JsonConvertidor<HabitacionModel>().Deserializar(endpoint);

                        if (resultRoom != null && resultRoom.Success)
                        {
                            TempData["Success"] = resultRoom.Message;
                            return PartialView("_List", new List<HabitacionModel> { resultRoom.Data });
                        }
                        else
                        {
                            TempData["Error"] = resultRoom.Message;
                            return PartialView("_List", new List<HabitacionModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("Habitacion/Get-Habitaciones");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointList.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointList.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultList = await new JsonConvertidor<HabitacionModel>().DeserializarList(endpointList);

                        if (resultList != null && resultList.Success)
                        {
                            TempData["Success"] = resultList.Message;
                            return PartialView("_List", resultList.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultList.Message;
                            return PartialView("_List", new List<HabitacionModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener las habitaciones.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- GET: Crear habitación (vista completa) ---
        public IActionResult Create()
        {
            var model = new CreateHabitacionModel();
            return View(model);
        }

        // --- POST: Crear habitación ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHabitacionModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PostAsJsonAsync("Habitacion/Create-Habitacion", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultRoom = await new JsonConvertidor<HabitacionModel>().Deserializar(endpoint);

                    if (resultRoom != null && resultRoom.Success)
                    {
                        TempData["Success"] = resultRoom.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = resultRoom.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al crear la habitación.";
                return View("Error", ex.Message);
            }
        }

        // --- GET: Editar habitación ---
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Habitacion/Get-Habitacion-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultRoom = await new JsonConvertidor<UpdateHabitacionModel>().Deserializar(endpoint);

                    if (resultRoom != null && resultRoom.Success)
                    {
                        TempData["Success"] = resultRoom.Message;
                        return View(resultRoom.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultRoom.Message;
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

        // --- POST: Editar habitación ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateHabitacionModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.PutAsJsonAsync("Habitacion/Update-Habitacion", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultRoom = await new JsonConvertidor<HabitacionModel>().Deserializar(endpoint);

                    if (resultRoom != null && resultRoom.Success)
                    {
                        TempData["Success"] = resultRoom.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = resultRoom.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar la habitación.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial DELETE ---
        public async Task<IActionResult> _Delete(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Habitacion/Get-Habitacion-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultRoom = await new JsonConvertidor<HabitacionModel>().Deserializar(endpoint);

                    if (resultRoom != null && resultRoom.Success)
                    {
                        TempData["Success"] = resultRoom.Message;
                        return PartialView("_Delete", resultRoom.Data);
                    }
                    else
                    {
                        TempData["Error"] = resultRoom.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al eliminar la habitación.";
                return View("Error", ex.Message);
            }
        }

        // --- Confirmar DELETE ---
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Habitacion/Remove-Habitacion?id={id}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointRemove.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointRemove.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<HabitacionModel>().Deserializar(endpointRemove);

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
                ViewBag.Error = "Error interno al eliminar la habitación.";
                return View("Error", ex.Message);
            }
        }










        /*private readonly IHabitacionServices _habitacionServices;

        public HabitacionAPIController(IHabitacionServices habitacionServices)
        {
            _habitacionServices = habitacionServices;
        }

        // GET: HabitacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HabitacionController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _habitacionServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var habitacion = result.Data as HabitacionDto;
            return View(habitacion); // Vista completa
        }

        //GET: Partial para listar Habitaciones
        public async Task<IActionResult> _List(string? numeroHabitacion)
        {
            // 3️⃣ Filtro por número de habitación
            if (!string.IsNullOrEmpty(numeroHabitacion))
            {
                var habitacion = await _habitacionServices.GetByNumero(numeroHabitacion);
                if (!habitacion.Success)
                {
                    
                    return PartialView("_Error", habitacion.Message);
                }
                
                var list = new List<HabitacionDto> { habitacion.Data };
                return PartialView("_List", list);
            }
                
            else
            {
                var result = await _habitacionServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaHabitaciones = result.Data as IEnumerable<HabitacionDto>;
                return PartialView("_List", listaHabitaciones);
            }
        }

        // GET: HabitacionController/Create
        public IActionResult Create()
        {
            var model = new CreateHabitacionDto();
            return View(model); // Vista completa
        }

        // POST: HabitacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHabitacionDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _habitacionServices.CreateAsync(dto);
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

        // GET: HabitacionController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _habitacionServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateHabitacionDto habitacion = new UpdateHabitacionDto
            {
                Id = result.Data.Id,
                Numero = result.Data.Numero,
                NumeroPiso = result.Data.NumeroPiso,
                Capacidad = result.Data.Capacidad,    
                CategoriaName = result.Data.CategoriaName,
                AmenityName = result.Data.AmenityName,
                Estado = result.Data.Estado
            };
            return View(habitacion); // Vista completa
        }

        // POST: HabitacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateHabitacionDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _habitacionServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: HabitacionController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _habitacionServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Habitación no encontrada.";
                return PartialView("_Error");
            }
            TempData["Success"] = result.Message;
            return PartialView("_Delete", (HabitacionDto)result.Data);

        }

        // POST: HabitacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _habitacionServices.DeleteAsync(id);
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
