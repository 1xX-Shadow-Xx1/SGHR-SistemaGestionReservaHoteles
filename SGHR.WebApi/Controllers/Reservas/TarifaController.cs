using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Tarifa;
using SGHR.Application.Interfaces.Reservas;

namespace SGHR.Web.Controllers.Reservas
{
    public class TarifaController : Controller
    {
        private readonly ITarifaServices _tarifaServices;

        public TarifaController(ITarifaServices tarifaServices)
        {
            _tarifaServices = tarifaServices;
        }

        // GET: TarifaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TarifaController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _tarifaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var tarifa = result.Data as TarifaDto;
            return View(tarifa); // Vista completa
        }

        //GET: Partial para listar tarifa
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _tarifaServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<TarifaDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<TarifaDto> { (TarifaDto)result.Data });
            }
            else
            {
                var result = await _tarifaServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaTarifas = result.Data as IEnumerable<TarifaDto>;
                return PartialView("_List", listaTarifas);
            }
        }

        // GET: TarifaController/Create
        public IActionResult Create()
        {
            var model = new CreateTarifaDto();
            return View(model); // Vista completa
        }

        // POST: TarifaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTarifaDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _tarifaServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de tarifas o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: TarifaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _tarifaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateTarifaDto tarifa = new UpdateTarifaDto
            {
                Id = result.Data.Id,
                NombreCategoria = result.Data.NombreCategoria,
                Fecha_inicio = result.Data.Fecha_inicio,
                Fecha_fin = result.Data.Fecha_fin,
                Precio = result.Data.Precio
            };
            return View(tarifa); // Vista completa
        }

        // POST: TarifaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTarifaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _tarifaServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: TarifaController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _tarifaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Tarifa no encontrada.";
                return PartialView("_Error");
            }
            return PartialView("_Delete", (TarifaDto)result.Data);

        }

        // POST: TarifaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _tarifaServices.DeleteAsync(id);
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
