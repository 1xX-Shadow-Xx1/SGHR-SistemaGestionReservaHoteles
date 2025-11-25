using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    [Area("Administrador")]
    public class AmenityAPIController : Controller
    {
        private readonly IAmenityServiceAPI _amenityServiceAPI;

        public AmenityAPIController(IAmenityServiceAPI amenityServiceAPI)
        {
            _amenityServiceAPI = amenityServiceAPI;
        }

        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar amenities ---
        public IActionResult _List(int? id)
        {
            try
            {

                if (id.HasValue && id > 0)
                {
                    var result = _amenityServiceAPI.GetByIDServices(id.Value);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<AmenityModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<AmenityModel>());
                    }
                }
                else
                {
                    var result = _amenityServiceAPI.GetServices();
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
                var result = _amenityServiceAPI.GetByIDServices(id);

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

        // GET: Vista completa de creación
        public IActionResult Create()
        {
            var model = new CreateAmenityModel();
            return View(model);
        }

        // POST: Crear amenity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAmenityModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _amenityServiceAPI.SaveServicesPost(model);

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

        // GET: Editar amenity
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _amenityServiceAPI.GetByIDServices(id);

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

        // POST: Actualizar amenity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAmenityModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _amenityServiceAPI.UpdateServicesPut(model);

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
                var result = _amenityServiceAPI.GetByIDServices(id);

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

        // POST: Confirmar eliminación
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _amenityServiceAPI.RemoveServicesPut(id);

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












        /*private readonly IAmenityServices _amenityServices;

        public AmenityAPIController(IAmenityServices amenityServices)
        {
            _amenityServices = amenityServices;
        }

        // GET: AmenityController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AmenityController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _amenityServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var amenity = result.Data as AmenityDto;
            return View(amenity); // Vista completa
        }

        //GET: Partial para listar amenities
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _amenityServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<AmenityDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<AmenityDto> { (AmenityDto)result.Data });
            }
            else
            {
                var result = await _amenityServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaAmenities = result.Data as IEnumerable<AmenityDto>;
                return PartialView("_List", listaAmenities);
            }
        }

        // GET: AmenityController/Create
        public IActionResult Create()
        {
            var model = new CreateAmenityDto();
            return View(model); // Vista completa
        }

        // POST: AmenityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAmenityDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _amenityServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de amenities o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: AmenityController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _amenityServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateAmenityDto amenity = new UpdateAmenityDto
            {
                Id = result.Data.Id,
                Nombre = result.Data.Nombre,
                Descripcion = result.Data.Descripcion,
                Precio = result.Data.Precio,
                PorCapacidad = result.Data.PorCapacidad
            };
            return View(amenity); // Vista completa
        }

        // POST: AmenityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAmenityDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _amenityServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: AmenityController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _amenityServices.GetByIdAsync(id);
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

            return PartialView("_Delete", (AmenityDto)result.Data);

        }

        // POST: AmenityController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _amenityServices.DeleteAsync(id);
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
