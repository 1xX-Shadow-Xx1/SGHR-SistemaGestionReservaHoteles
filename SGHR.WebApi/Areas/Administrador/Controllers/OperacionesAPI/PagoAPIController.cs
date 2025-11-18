using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Operaciones.Pago;

namespace SGHR.Web.Areas.Administrador.Controllers.OperacionesAPI
{
    [Area("Administrador")]
    public class PagoAPIController : Controller
    {

        public PagoAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar pagos ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        // Suponiendo que quieres filtrar por cliente
                        var endpointPagoCliente = await httpclient.GetAsync($"Pago/Get-Pagos?idCliente={id}");

                        if (endpointPagoCliente.IsSuccessStatusCode)
                        {
                            string response = await endpointPagoCliente.Content.ReadAsStringAsync();
                            var resultPago = JsonConvert.DeserializeObject<ServicesResultModel<List<PagoModel>>>(response);

                            if (resultPago != null && resultPago.Success)
                            {
                                TempData["Success"] = resultPago.Message;
                                return PartialView("_List", resultPago.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultPago.Message;
                                return PartialView("_List", new List<PagoModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointPagoCliente.StatusCode}";
                            return PartialView("_List", new List<PagoModel>());
                        }
                    }
                    else
                    {
                        var endpointLista = await httpclient.GetAsync("Pago/Get-Pagos");

                        if (endpointLista.IsSuccessStatusCode)
                        {
                            string responseList = await endpointLista.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<PagoModel>>>(responseList);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<PagoModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointLista.StatusCode}";
                            return PartialView("_List", new List<PagoModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los pagos.";
                return PartialView("Error", ex.Message);
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
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Pago/Realizar-Pago", model);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var resultPago = JsonConvert.DeserializeObject<ServicesResultModel<PagoModel>>(response);

                        if (resultPago != null && resultPago.Success)
                        {
                            TempData["Success"] = resultPago.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultPago.Message;
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointCreate.StatusCode}";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al realizar el pago.";
                return View("Error", ex.Message);
            }
        }

        // GET: Detalles de pago
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Pago/Get-Pagos?id={id}");

                    if (endpointDetail.IsSuccessStatusCode)
                    {
                        string response = await endpointDetail.Content.ReadAsStringAsync();
                        var resultDetail = JsonConvert.DeserializeObject<ServicesResultModel<PagoModel>>(response);

                        if (resultDetail != null && resultDetail.Success)
                        {
                            TempData["Success"] = resultDetail.Message;
                            return View(resultDetail.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultDetail.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointDetail.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los detalles del pago.";
                return View("Error", ex.Message);
            }
        }

        // GET: Partial para anular pago
        public async Task<IActionResult> _AnularPago(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Pago/Get-PagosByID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultPago = JsonConvert.DeserializeObject<ServicesResultModel<PagoModel>>(response);

                        if (resultPago != null && resultPago.Success)
                        {
                            TempData["Success"] = resultPago.Message;
                            return PartialView("_AnularPago", resultPago.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultPago.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al mostrar el pago a anular.";
                return View("Error", ex.Message);
            }
        }

        // POST: Confirmar anulación
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AnularConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Pago/Anular-Pago?idPago={id}", null);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<PagoModel>>(response);
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<PagoModel>>(response);
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al anular el pago.";
                return View("Error", ex.Message);
            }
        }

        // GET: Partial Resumen de pagos
        public async Task<IActionResult> _resumenPagos()
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointResumen = await httpclient.GetAsync("Pago/Get-Resumen-Pagos");

                    if (endpointResumen.IsSuccessStatusCode)
                    {
                        string response = await endpointResumen.Content.ReadAsStringAsync();
                        var resultResumen = JsonConvert.DeserializeObject<ServicesResultModel<ResumenPagoModel>>(response);

                        if (resultResumen != null && resultResumen.Success)
                        {
                            TempData["Success"] = resultResumen.Message;
                            return PartialView(resultResumen.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultResumen.Message;
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointResumen.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener el resumen de pagos.";
                return View("Error", ex.Message);
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
