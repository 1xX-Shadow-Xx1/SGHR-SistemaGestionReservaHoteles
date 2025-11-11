using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Piso;
using SGHR.Application.Interfaces.Habitaciones;

namespace SGHR.Web.Controllers.Administrador.Habitaciones
{
    public class PisoController : Controller
    {
        private readonly IPisoServices _pisoServices;

        public PisoController(IPisoServices pisoServices)
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
        }
    }
}
