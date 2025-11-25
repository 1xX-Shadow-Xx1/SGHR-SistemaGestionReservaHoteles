using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Reservas.Reserva;
using SGHR.Web.Services.Interfaces.Reservas;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.ReservasAPI
{
    [Area("Administrador")]
    public class ReservaAPIController : Controller
    {
        private readonly IReservaServiceAPI _reservaServiceAPI;
        public ReservaAPIController(IReservaServiceAPI reservaServiceAPI)
        {
            _reservaServiceAPI = reservaServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar reservas ---
        public IActionResult _List(int? id)
        {
            try
            {

                if (id.HasValue && id > 0)
                {
                    var result = _reservaServiceAPI.GetByIDServices((int)id);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<ReservaModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<ReservaModel>());
                    }
                }
                else
                {
                    var result = _reservaServiceAPI.GetServices();
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

        // --- Vista completa de detalles de reserva ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _reservaServiceAPI.GetByIDServices(id);

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
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
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
                var result = await _reservaServiceAPI.SaveServicesPost(model);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Servicios", new { id = result.Data.Id });
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

        // GET: Editar reserva
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _reservaServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View(new UpdateReservaModel
                    {
                        Id = result.Data.Id,
                        CedulaCliente = result.Data.CedulaCliente,
                        Estado = result.Data.Estado,
                        CorreoCliente = result.Data.CorreoCliente,
                        FechaFin = result.Data.FechaFin,
                        FechaInicio = result.Data.FechaInicio,
                        NumeroHabitacion = result.Data.NumeroHabitacion
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

        // POST: Editar reserva
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReservaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _reservaServiceAPI.UpdateServicesPut(model);

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

        // --- Partial para eliminar ---
        public IActionResult _Delete(int id)
        {
            try
            {
                var result = _reservaServiceAPI.GetByIDServices(id);

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

        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _reservaServiceAPI.RemoveServicesPut(id);

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
                var result = await _reservaServiceAPI.GetServicesbyReserva(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return Json(new { success = true, data = result.Data });
                }
                else
                {
                    TempData["Error"] = $"Error {result.Message}";
                    return Json(new { success = false, message = $"Error {result.Message}" });
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // GET: Servicios disponibles
        [HttpGet]
        public IActionResult GetServiciosDisponibles()
        {
            try
            {
                var result = _reservaServiceAPI.GetServiciosAdicionalesdisponibles();

                TempData["Success"] = "Lista cargada correctamente.";
                return Json(new { success = true, data = result });
                
            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // POST: Agregar servicio a reserva
        [HttpPost]
        public async Task<IActionResult> AgregarServicio(int idReserva, string nombreServicio)
        {
            try
            {
                var result = await _reservaServiceAPI.AddServicio_ReservaPut(nombreServicio, idReserva);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return Json(new { success = false, message = result.Message });
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // POST: Remover servicio de reserva
        [HttpPost]
        public async Task<IActionResult> RemoverServicio(int idReserva, string nombreServicio)
        {
            try
            {
                var result = await _reservaServiceAPI.RemoveServicio_ReservaPut(nombreServicio, idReserva);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return Json(new { success = true, message = result.Message });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return Json(new { success = false, message = result.Message });
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
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
