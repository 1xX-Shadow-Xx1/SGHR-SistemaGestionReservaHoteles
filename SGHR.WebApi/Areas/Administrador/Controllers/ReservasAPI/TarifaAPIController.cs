using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Reservas.Tarifa;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Validador;
using System.Net;

namespace SGHR.Web.Areas.Administrador.Controllers.ReservasAPI
{
    [Area("Administrador")]
    public class TarifaAPIController : Controller
    {
        public TarifaAPIController()
        {
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar tarifas ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    // --- Buscar por ID ---
                    if (id.HasValue && id > 0)
                    {
                        var endpointTarifa = await httpclient.GetAsync($"Tarifa/Get-Tarifa-ByID?id={id}");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointTarifa.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointTarifa.StatusCode, ErrorMessage = errorMessage });
                        }

                        var result = await new JsonConvertidor<TarifaModel>().Deserializar(endpointTarifa);

                        if (result != null && result.Success)
                        {
                            TempData["Success"] = result.Message;
                            return PartialView("_List", new List<TarifaModel> { result.Data });
                        }
                        else
                        {
                            TempData["Error"] = result.Message;
                            return PartialView("_List", new List<TarifaModel>());
                        }
                    }
                    else
                    {
                        // --- Listado completo ---
                        var endpointList = await httpclient.GetAsync("Tarifa/Get-Tarifas");

                        var validate = new ValidateStatusCode().ValidatorStatus((int)endpointList.StatusCode, out string errorMessage);
                        if (!validate && errorMessage != string.Empty)
                        {
                            ViewBag.Error = errorMessage;
                            return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointList.StatusCode, ErrorMessage = errorMessage });
                        }

                        var resultList = await new JsonConvertidor<TarifaModel>().DeserializarList(endpointList);

                        if (resultList != null && resultList.Success)
                        {
                            TempData["Success"] = resultList.Message;
                            return PartialView("_List", resultList.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultList.Message;
                            return PartialView("_List", new List<TarifaModel>());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener las tarifas.";
                return PartialView("Error", ex.Message);
            }
        }

        // --- Vista completa de detalles ---
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointDetail = await httpclient.GetAsync($"Tarifa/Get-Tarifa-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointDetail.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointDetail.StatusCode, ErrorMessage = errorMessage });
                    }

                    var resultDetail = await new JsonConvertidor<TarifaModel>().Deserializar(endpointDetail);

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
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener la tarifa.";
                return View("Error", ex.Message);
            }
        }

        // GET: Crear tarifa
        public IActionResult Create()
        {
            var model = new CreateTarifaModel();
            return View(model);
        }

        // POST: Crear tarifa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTarifaModel model)
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
                    var endpointCreate = await httpclient.PostAsJsonAsync("Tarifa/Create-Tarifa", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointCreate.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointCreate.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<TarifaModel>().Deserializar(endpointCreate);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al crear la tarifa.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar tarifa
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.GetAsync($"Tarifa/Get-Tarifa-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<UpdateTarifaModel>().Deserializar(endpointEdit);

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
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al mostrar la vista de editar.";
                return View("Error", ex.Message);
            }
        }

        // POST: Editar tarifa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTarifaModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Tarifa/Update-Tarifa", model);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointEdit.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointEdit.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<TarifaModel>().Deserializar(endpointEdit);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar la tarifa.";
                return View("Error", ex.Message);
            }
        }

        // --- Partial para eliminar ---
        public async Task<IActionResult> _Delete(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Tarifa/Get-Tarifa-ByID?id={id}");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<TarifaModel>().Deserializar(endpoint);

                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_Delete", result.Data);
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al eliminar la tarifa.";
                return View("Error", ex.Message);
            }
        }

        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Tarifa/Remove-Tarifa?id={id}", null);

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpointRemove.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpointRemove.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<TarifaModel>().Deserializar(endpointRemove);

                    if (result != null && result.Success)
                    {
                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar la tarifa.";
                return View("Error", ex.Message);
            }
        }









        /*private readonly ITarifaServices _tarifaServices;

        public TarifaAPIController(ITarifaServices tarifaServices)
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

                    return PartialView("_List", new List<TarifaModel>()); // lista vacía si no se encuentra
                }

                return PartialView("_List", new List<TarifaModel> { (TarifaModel)result.Data });
            }
            else
            {
                var result = await _tarifaServices.GetAllAsync();
                if (!result.Success)
                {

                    return PartialView("_Error", result.Message);
                }
                var listaTarifas = result.Data as IEnumerable<TarifaModel>;
                return PartialView("_List", listaTarifas);
            }
        }

        // GET: TarifaController/Create
        public IActionResult Create()
        {
            var model = new CreateTarifaModel();
            return View(model); // Vista completa
        }

        // POST: TarifaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTarifaModel dto)
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
           UpdateTarifaModel tarifa = new UpdateTarifaModel
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
        public async Task<IActionResult> Edit(UpdateTarifaModel dto)
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
            return PartialView("_Delete", (TarifaModel)result.Data);

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
        }*/
    }
}
