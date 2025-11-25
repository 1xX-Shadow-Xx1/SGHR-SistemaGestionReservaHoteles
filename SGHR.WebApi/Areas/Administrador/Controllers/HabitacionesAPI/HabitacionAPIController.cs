using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Habitacion;
using SGHR.Web.Models.Habitaciones.Piso;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    
    [Area("Administrador")]
    public class HabitacionAPIController : Controller
    {
        private readonly IHabitacionServiceAPI _habitacionServiceAPI;
        public HabitacionAPIController(IHabitacionServiceAPI habitacionServiceAPI)
        {
            _habitacionServiceAPI = habitacionServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Vista completa de detalles de la habitación ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _habitacionServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

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
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener la habitación.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial para listar habitaciones ---
        public IActionResult _List(string? numeroHabitacion)
        {
            try
            {

                if (!string.IsNullOrEmpty(numeroHabitacion))
                {
                    var result = _habitacionServiceAPI.GetHabitacionByNumero(numeroHabitacion);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<HabitacionModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<HabitacionModel>());
                    }
                }
                else
                {
                    var result = _habitacionServiceAPI.GetServices();
                    TempData["Success"] = "Lista cargada correctamente.";
                    return PartialView("_List", result);

                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
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
                var result = await _habitacionServiceAPI.SaveServicesPost(model);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

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
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // --- GET: Editar habitación ---
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _habitacionServiceAPI.GetByIDServices(id);
                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View(new UpdateHabitacionModel
                    {
                        Id = result.Data.Id,
                        Numero = result.Data.Numero,
                        NumeroPiso = result.Data.NumeroPiso,
                        Estado = result.Data.Estado,
                        AmenityName = result.Data.AmenityName,
                        Capacidad = result.Data.Capacidad,
                        CategoriaName = result.Data.CategoriaName
                    });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
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
                var result = await _habitacionServiceAPI.UpdateServicesPut(model);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

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
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // --- Partial DELETE ---
        public IActionResult _Delete(int id)
        {
            try
            {
                var result = _habitacionServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

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
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // --- Confirmar DELETE ---
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _habitacionServiceAPI.RemoveServicesPut(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    return PartialView("_DeleteSuccess", result.Message);
                }
                else
                {
                    return PartialView("_DeleteError", result.Message);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
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
