using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Operaciones.Pago;
using SGHR.Web.Services.Interfaces.Operaciones;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.OperacionesAPI
{
    [Area("Administrador")]
    public class PagoAPIController : Controller
    {
        private readonly IPagoServiceAPI _pagoServiceAPI;
        public PagoAPIController(IPagoServiceAPI pagoServiceAPI)
        {
            _pagoServiceAPI = pagoServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar pagos ---
        public IActionResult _List(int? id)
        {
            try
            {

                if (id.HasValue && id > 0)
                {
                    // Suponiendo que quieres filtrar por cliente
                    var result = _pagoServiceAPI.getPagoById(id.Value);

                    var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return Json(new
                        {
                            redirectToError = true,
                            statusCode = result.Statuscode,
                            errorMessage = errorMessage
                        });
                    }

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<PagoModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<PagoModel>());
                    }
                }
                else
                {
                    var result = _pagoServiceAPI.getPagoList();
                    TempData["Success"] = "Lista cargada correctamente.";
                    return PartialView("_List", result);


                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return Json(new
                {
                    redirectToError = true,
                    statusCode = 500,
                    errorMessage = errorMessage
                });
            }
        }

        // GET: Vista completa para realizar pago
        public IActionResult RealizarPago()
        {
            var model = new RealizarPagoModel();
            return View(model);
        }

        // POST: Realizar pago
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PagoConfirmed(RealizarPagoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _pagoServiceAPI.RealizarPago(model);

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
                    return View("RealizarPago", model);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // GET: Detalles de pago
        public IActionResult Details(int id)
        {
            try
            {
                var result = _pagoServiceAPI.getPagoById(id);

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

        // GET: Partial para anular pago
        public IActionResult _AnularPago(int id)
        {
            try
            {
                var result = _pagoServiceAPI.getPagoById(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return Json(new
                    {
                        redirectToError = true,
                        statusCode = result.Statuscode,
                        errorMessage = errorMessage
                    });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return PartialView("_AnularPago", result.Data);
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
                return Json(new
                {
                    redirectToError = true,
                    statusCode = 500,
                    errorMessage = errorMessage
                });
            }
        }

        // POST: Confirmar anulación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnularConfirmed(int id)
        {
            try
            {
                var result = await _pagoServiceAPI.AnularPago(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return Json(new
                    {
                        redirectToError = true,
                        statusCode = result.Statuscode,
                        errorMessage = errorMessage
                    });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return PartialView("_AnularSuccess", result.Message);
                }
                else
                {
                    TempData["Success"] = result.Message;
                    return PartialView("_AnularError", result.Message);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return Json(new
                {
                    redirectToError = true,
                    statusCode = 500,
                    errorMessage = errorMessage
                });
            }
        }

        // GET: Partial Resumen de pagos
        public async Task<IActionResult> _resumenPagos()
        {
            try
            {
                var result = await _pagoServiceAPI.GetResumenDePagos();

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return Json(new
                    {
                        redirectToError = true,
                        statusCode = result.Statuscode,
                        errorMessage = errorMessage
                    });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return PartialView(result.Data);
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
                return Json(new
                {
                    redirectToError = true,
                    statusCode = 500,
                    errorMessage = errorMessage
                });
            }
        }










        /*private readonly IPagoServices _pagoServices;

        public PagoAPIController(IPagoServices pagoServices)
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
                return RedirectToAction("RealizarPago");
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
        }*/
    }
}
