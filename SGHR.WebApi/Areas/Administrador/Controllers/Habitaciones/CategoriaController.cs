using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Categoria;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Web.Models.Habitaciones.Categoria;

namespace SGHR.Web.Areas.Administrador.Controllers.Habitaciones
{
    [Area("Administrador")]
    public class CategoriaController : Controller
    {
        private readonly ICategoriaServices _categoriaServices;

        public CategoriaController(ICategoriaServices categoriaServices)
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
        }
    }
}
