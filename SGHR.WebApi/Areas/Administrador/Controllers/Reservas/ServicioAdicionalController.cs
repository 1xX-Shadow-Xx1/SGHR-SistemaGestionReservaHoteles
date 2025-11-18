using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.ServicioAdicional;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Web.Models.Reservas.ServicioAdicional;

namespace SGHR.Web.Areas.Administrador.Controllers.Reservas
{
    [Area("Administrador")]
    public class ServicioAdicionalController : Controller
    {
        private readonly IServicioAdicionalServices _servicioAdicionalServices;

        public ServicioAdicionalController(IServicioAdicionalServices servicioAdicionalServices)
        {
            _servicioAdicionalServices = servicioAdicionalServices;
        }

        // GET: ServicioAdicionalController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServicioAdicionalController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var reserva = result.Data as ServicioAdicionalDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar servicios adicionales
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _servicioAdicionalServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ServicioAdicionalDto>()); // lista vacía si no se encuentra
                }
                return PartialView("_List", new List<ServicioAdicionalDto> { (ServicioAdicionalDto)result.Data });
            }
            else
            {
                var result = await _servicioAdicionalServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaServicios = result.Data as IEnumerable<ServicioAdicionalDto>;
                return PartialView("_List", listaServicios);
            }
        }

        // GET: ServicioAdicionalController/Create
        public IActionResult Create()
        {
            var model = new CreateServicioAdicionalDto();
            return View(model); // Vista completa
        }

        // POST: ServicioAdicionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServicioAdicionalDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _servicioAdicionalServices.CreateAsync(dto);
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

        // GET: ServicioAdicionalController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateServicioAdicionalDto servicio = new UpdateServicioAdicionalDto
            {
                Id = result.Data.Id,
                Nombre = result.Data.Nombre,
                Descripcion = result.Data.Descripcion,
                Precio = result.Data.Precio,
                Estado = result.Data.Estado
            };
            return View(servicio); // Vista completa
        }

        // POST: ServicioAdicionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateServicioAdicionalDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _servicioAdicionalServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ServicioAdicionalController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _servicioAdicionalServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Servicio no encontrado.";
                return PartialView("_Error");
            }

            return PartialView("_Delete", (ServicioAdicionalDto)result.Data);

        }

        // POST: ServicioAdicionalController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _servicioAdicionalServices.DeleteAsync(id);
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
