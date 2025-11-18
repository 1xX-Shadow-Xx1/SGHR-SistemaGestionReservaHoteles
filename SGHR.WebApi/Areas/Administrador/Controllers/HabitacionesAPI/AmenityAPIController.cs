using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;

namespace SGHR.Web.Areas.Administrador.Controllers.HabitacionesAPI
{
    [Area("Administrador")]
    public class AmenityAPIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar amenities ---
        public async Task<IActionResult> _List(int? id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    if (id.HasValue && id > 0)
                    {
                        var endpointAmenity = await httpclient.GetAsync($"Amenity/Get-By-ID?id={id}");

                        if (endpointAmenity.IsSuccessStatusCode)
                        {
                            string response = await endpointAmenity.Content.ReadAsStringAsync();
                            var resultAmenity = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                            if (resultAmenity != null && resultAmenity.Success)
                            {
                                TempData["Success"] = resultAmenity.Message;
                                return PartialView("_List", new List<AmenityModel> { resultAmenity.Data });
                            }
                            else
                            {
                                TempData["Error"] = resultAmenity.Message;
                                return PartialView("_List", new List<AmenityModel>());
                            }

                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointAmenity.StatusCode}";
                            return PartialView("_List", new List<AmenityModel>());
                        }
                    }
                    else
                    {
                        var endpointList = await httpclient.GetAsync("Amenity/Get-Amenity");

                        if (endpointList.IsSuccessStatusCode)
                        {
                            string responseList = await endpointList.Content.ReadAsStringAsync();
                            var resultList = JsonConvert.DeserializeObject<ServicesResultModel<List<AmenityModel>>>(responseList);

                            if (resultList != null && resultList.Success)
                            {
                                TempData["Success"] = resultList.Message;
                                return PartialView("_List", resultList.Data);
                            }
                            else
                            {
                                TempData["Error"] = resultList.Message;
                                return PartialView("_List", new List<AmenityModel>());
                            }
                        }
                        else
                        {
                            TempData["Error"] = $"Error {endpointList.StatusCode}";
                            return PartialView("_List", new List<AmenityModel>());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener los amenities.";
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
                    var endpoint = await httpclient.GetAsync($"Amenity/Get-By-ID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

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
                    else
                    {
                        TempData["Error"] = $"Error {endpoint.StatusCode}";
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error interno al obtener el amenity.";
                return View("Error", ex.Message);
            }
        }

        // GET: Vista completa de creación
        public IActionResult Create()
        {
            var model = new CreateAmenityModel();
            return View(model);
        }

        // POST: Crear amenity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAmenityModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointCreate = await httpclient.PostAsJsonAsync("Amenity/Create-Amenity", model);

                    if (endpointCreate.IsSuccessStatusCode)
                    {
                        string response = await endpointCreate.Content.ReadAsStringAsync();
                        var resultAmenity = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                        if (resultAmenity != null && resultAmenity.Success)
                        {
                            TempData["Success"] = resultAmenity.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultAmenity.Message;
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
                ViewBag.Error = "Ocurrió un error interno al crear el amenity.";
                return View("Error", ex.Message);
            }
        }

        // GET: Editar amenity
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpoint = await httpclient.GetAsync($"Amenity/Get-By-ID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultAmenity = JsonConvert.DeserializeObject<ServicesResultModel<UpdateAmenityModel>>(response);

                        if (resultAmenity != null && resultAmenity.Success)
                        {
                            TempData["Success"] = resultAmenity.Message;
                            return View(resultAmenity.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultAmenity.Message;
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
                ViewBag.Error = "Error interno al mostrar la vista de editar.";
                return View("Error", ex.Message);
            }
        }

        // POST: Actualizar amenity
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAmenityModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointEdit = await httpclient.PutAsJsonAsync("Amenity/Update-Amenity", model);

                    if (endpointEdit.IsSuccessStatusCode)
                    {
                        string response = await endpointEdit.Content.ReadAsStringAsync();
                        var resultAmenity = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                        if (resultAmenity != null && resultAmenity.Success)
                        {
                            TempData["Success"] = resultAmenity.Message;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Error"] = resultAmenity.Message;
                            return View(model);
                        }
                    }
                    else
                    {
                        TempData["Error"] = $"Error {endpointEdit.StatusCode}";
                        return View(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al actualizar el amenity.";
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
                    var endpoint = await httpclient.GetAsync($"Amenity/Get-By-ID?id={id}");

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var resultAmenity = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                        if (resultAmenity != null && resultAmenity.Success)
                        {
                            TempData["Success"] = resultAmenity.Message;
                            return PartialView("_Delete", resultAmenity.Data);
                        }
                        else
                        {
                            TempData["Error"] = resultAmenity.Message;
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
                ViewBag.Error = "Ocurrió un error interno al eliminar el amenity.";
                return View("Error", ex.Message);
            }
        }

        // POST: Confirmar eliminación
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");
                    var endpointRemove = await httpclient.PutAsync($"Amenity/Remove-Amenity?id={id}", null);

                    if (endpointRemove.IsSuccessStatusCode)
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                        return Json(new { success = true, message = result.Message, data = result.Data });
                    }
                    else
                    {
                        string response = await endpointRemove.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<AmenityModel>>(response);

                        return Json(new { success = false, message = $"Error {result.Message}" });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error interno al eliminar el amenity.";
                return View("Error", ex.Message);
            }
        }












        /*private readonly IAmenityServices _amenityServices;

        public AmenityAPIController(IAmenityServices amenityServices)
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
        }*/
    }
}
