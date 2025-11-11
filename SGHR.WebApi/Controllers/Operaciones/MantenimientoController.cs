using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Mantenimiento;
using SGHR.Application.Interfaces.Operaciones;

namespace SGHR.Web.Controllers.Operaciones
{
    public class MantenimientoController : Controller
    {
        private readonly IMantenimientoServices _mantenimientoServices;

        public MantenimientoController(IMantenimientoServices mantenimientoServices)
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
        }
    }
}
