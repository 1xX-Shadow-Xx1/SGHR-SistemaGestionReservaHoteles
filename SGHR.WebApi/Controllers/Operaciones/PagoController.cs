using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;

namespace SGHR.Web.Controllers.Operaciones
{
    public class PagoController : Controller
    {

        private readonly IPagoServices _pagoServices;

        public PagoController(IPagoServices pagoServices)
        {
            _pagoServices = pagoServices;
        }

        // GET: PagoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PagoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _pagoServices.GetPagoByIdAsync(id);
            if (!result.Success)
            {
                return RedirectToAction("Index");
            }

            var pago = result.Data as PagoDto;
            return View(pago); // Vista completa
        }

        //GET: Partial para listar pagos
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _pagoServices.GetPagoByCliente(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<PagoDto>());
                }
                
                var listaPagosCliente = result.Data as IEnumerable<PagoDto>;
                return PartialView("_List", listaPagosCliente);
            }
            else
            {
                var result = await _pagoServices.ObtenerPagosAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaPagos = result.Data as IEnumerable<PagoDto>;
                return PartialView("_List", listaPagos);
            }
        }

        // GET: PagoController/RealizarPago
        public IActionResult RealizarPago()
        {
            var model = new RealizarPagoDto();
            return View(model); // Vista completa
        }

        // POST: PagoController/PagoConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PagoConfirmed(RealizarPagoDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _pagoServices.RealizarPagoAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de pagos o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: partial view PagoController/_AnularPago/5
        public async Task<IActionResult> _AnularPago(int id)
        {
            var result = await _pagoServices.GetPagoByIdAsync(id);
            if (!result.Success)
            {
                TempData["Success"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            return PartialView("_AnularPago", (PagoDto)result.Data);
        }

        // POST: PagoController/AnularConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnularConfirmed(int id)
        {
            var result = await _pagoServices.AnularPagoAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return Json(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }
            TempData["Success"] = result.Message;
            return Json(new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        // GET: partial view PagoController/ResumenPagos/5
        public async Task<IActionResult> _resumenPagos()
        {
            ServiceResult result = await _pagoServices.ObtenerResumenPagosAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var resumenPagos = result.Data as ResumenPagoDto;
            return PartialView(resumenPagos); // Vista completa
        }
    }
}
