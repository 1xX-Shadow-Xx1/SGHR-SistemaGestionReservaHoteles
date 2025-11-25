using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Mantenimiento;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Services.Interfaces.Operaciones;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.OperacionesAPI
{
    [Area("Administrador")]
    public class MantenimientoAPIController : Controller
    {
        private readonly IMantenimientoServiceAPI _mantenimientoServiceAPI;
        public MantenimientoAPIController(IMantenimientoServiceAPI mantenimientoServiceAPI)
        {
            _mantenimientoServiceAPI = mantenimientoServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar mantenimientos ---
        public IActionResult _List(int? id)
        {
            try
            {

                if (id.HasValue && id > 0)
                {
                    var result = _mantenimientoServiceAPI.GetByIDServices(id.Value);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<MantenimientoModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<MantenimientoModel>());
                    }
                }
                else
                {
                    var result = _mantenimientoServiceAPI.GetServices();
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

        // --- Vista completa de detalles ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _mantenimientoServiceAPI.GetByIDServices(id);

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
                var result = await _mantenimientoServiceAPI.SaveServicesPost(model);

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

        // GET: Editar mantenimiento
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _mantenimientoServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View(new UpdateMantenimientoModel
                    {
                        Id = result.Data.Id,
                        RealizadoPor = result.Data.RealizadoPor,
                        Descripcion = result.Data.Descripcion,
                        NumeroHabitacion = result.Data.NumeroHabitacion,
                        NumeroPiso = result.Data.NumeroPiso,
                        Estado = result.Data.Estado,
                        FechaInicio = result.Data.FechaInicio,
                        FechaFin = result.Data.FechaFin
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

        // POST: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMantenimientoModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _mantenimientoServiceAPI.UpdateServicesPut(model);

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
                var result = _mantenimientoServiceAPI.GetByIDServices(id);

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
                var result = await _mantenimientoServiceAPI.RemoveServicesPut(id);

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
