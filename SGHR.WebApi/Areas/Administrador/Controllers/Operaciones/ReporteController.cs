using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Reporte;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Web.Models.Operaciones.Reporte;

namespace SGHR.Web.Areas.Administrador.Controllers.Operaciones
{
    [Area("Administrador")]
    public class ReporteController : Controller
    {
        private readonly IReporteServices _reporteServices;

        public ReporteController(IReporteServices reporteServices)
        {
            _reporteServices = reporteServices;
        }

        // GET: ReporteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReporteController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _reporteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }
            var reserva = result.Data as ReporteDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar reportes
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _reporteServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ReporteDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<ReporteDto> { (ReporteDto)result.Data });
            }
            else
            {
                var result = await _reporteServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaReportes = result.Data as IEnumerable<ReporteDto>;
                return PartialView("_List", listaReportes);
            }
        }

        // GET: ReporteController/Create
        public IActionResult Create()
        {
            var model = new CreateReporteDto();
            return View(model); // Vista completa
        }

        // POST: ReporteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReporteDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _reporteServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de reportes o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ReporteController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _reporteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateReporteDto reporte = new UpdateReporteDto
            {
                Id = result.Data.Id,
                GeneradoPor = result.Data.GeneradoPor,
                RutaArchivo = result.Data.RutaArchivo,
                TipoReporte = result.Data.TipoReporte
            };
            return View(reporte); // Vista completa
        }

        // POST: ReporteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReporteDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _reporteServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ReporteController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _reporteServices.GetByIdAsync(id);
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
                

            return PartialView("_Delete", (ReporteDto)result.Data);

        }

        // POST: ReporteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _reporteServices.DeleteAsync(id);
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
