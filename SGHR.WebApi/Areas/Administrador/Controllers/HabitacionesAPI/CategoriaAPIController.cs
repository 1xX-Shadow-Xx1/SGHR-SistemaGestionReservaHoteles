using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Habitaciones.Categoria;
using SGHR.Web.Services.Interfaces.Habitaciones;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    [Area("Administrador")]
    public class CategoriaAPIController : Controller
    {
        private readonly ICategoriaServiceAPI _categoriaServiceAPI;
        public CategoriaAPIController(ICategoriaServiceAPI categoriaServiceAPI)
        {
            _categoriaServiceAPI = categoriaServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar categorias ---
        public IActionResult _List(int? id)
        {
            try
            {


                if (id.HasValue && id > 0)
                {
                    var result = _categoriaServiceAPI.GetByIDServices(id.Value);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<CategoriaModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<CategoriaModel>());
                    }
                }
                else
                {
                    var result = _categoriaServiceAPI.GetServices();
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

        // --- Vista completa de detalles de categoria ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _categoriaServiceAPI.GetByIDServices(id);

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

        // GET: vista completa de creación
        public IActionResult Create()
        {
            var model = new CreateCategoriaModel();
            return View(model);
        }

        // POST: creación de categoría
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoriaModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _categoriaServiceAPI.SaveServicesPost(model);

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

        // GET: Editar categoria
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _categoriaServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View(new UpdateCategoriaModel
                    {
                        Id = result.Data.Id,
                        Nombre = result.Data.Nombre,
                        Precio = result.Data.Precio,
                        Descripcion = result.Data.Descripcion
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

        // POST: Actualizar categoria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoriaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _categoriaServiceAPI.UpdateServicesPut(model);

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
                var result = _categoriaServiceAPI.GetByIDServices(id);

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

        // Confirmación de eliminación
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _categoriaServiceAPI.RemoveServicesPut(id);

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













        /*private readonly ICategoriaServices _categoriaServices;

        public CategoriaAPIController(ICategoriaServices categoriaServices)
        {
            _categoriaServices = categoriaServices;
        }

        // GET: CategoriaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CategoriaController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _categoriaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }
            var categoria = result.Data as CategoriaDto;
            return View(categoria); // Vista completa
        }

        //GET: Partial para listar Categorias
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _categoriaServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<CategoriaDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<CategoriaDto> { (CategoriaDto)result.Data });
            }
            else
            {
                var result = await _categoriaServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }

                var listaCategorias = result.Data as IEnumerable<CategoriaDto>;
                return PartialView("_List", listaCategorias);
            }
        }

        // GET: CategoriaController/Create
        public IActionResult Create()
        {
            var model = new CreateCategoriaDto();
            return View(model); // Vista completa
        }

        // POST: CategoriaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoriaDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _categoriaServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de categorias o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: CategoriaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _categoriaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateCategoriaDto categoria = new UpdateCategoriaDto
            {
                Id = result.Data.Id,
                Nombre = result.Data.Nombre,
                Descripcion = result.Data.Descripcion,
                Precio = result.Data.Precio
            };
            return View(categoria); // Vista completa
        }

        // POST: CategoriaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoriaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _categoriaServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: CategoriaController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _categoriaServices.GetByIdAsync(id);
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

            return PartialView("_Delete", (CategoriaDto)result.Data);

        }

        // POST: CategoriaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _categoriaServices.DeleteAsync(id);
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
