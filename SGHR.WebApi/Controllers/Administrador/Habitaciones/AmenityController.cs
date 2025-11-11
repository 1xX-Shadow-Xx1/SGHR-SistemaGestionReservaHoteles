using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Habitaciones.Amenity;
using SGHR.Application.Interfaces.Habitaciones;

namespace SGHR.Web.Controllers.Administrador.Habitaciones
{
    public class AmenityController : Controller
    {
        private readonly IAmenityServices _amenityServices;

        public AmenityController(IAmenityServices amenityServices)
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
        }
    }
}
