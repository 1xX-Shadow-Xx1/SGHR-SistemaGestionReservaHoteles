using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Habitacion;
using SGHR.Application.Interfaces.Habitaciones;
using SGHR.Web.Models.Habitaciones.Habitacion;

namespace SGHR.Web.Areas.Administrador.Controllers.Habitaciones
{
    [Area("Administrador")]
    public class HabitacionController : Controller
    {
        private readonly IHabitacionServices _habitacionServices;

        public HabitacionController(IHabitacionServices habitacionServices)
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
        }
    }
}
