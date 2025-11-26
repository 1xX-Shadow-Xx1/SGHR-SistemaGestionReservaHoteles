using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Data;
using SGHR.Web.Models.Usuarios.Cliente;
using SGHR.Web.Services.Interfaces.Usuarios;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.UsuariosAPI
{
    [Area("Administrador")]
    public class ClienteAPIController : Controller
    {
        private readonly IClienteServiceAPI _clientServiceAPI;

        public ClienteAPIController(IClienteServiceAPI clientServiceAPI)
        {
            _clientServiceAPI = clientServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar clientes ---
        public IActionResult _List(string? cedula)
        {
            try
            {

                if (!string.IsNullOrEmpty(cedula))
                {
                    
                    var result = _clientServiceAPI.GetByCedulaCliente(cedula);
                    if (result != null && result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<ClienteModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<ClienteModel>());
                    }

                }
                else
                {
                    var result = _clientServiceAPI.GetServices();
                    TempData["Success"] = "Lista cargada correctamente.";
                    return PartialView("_List", result);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // --- Vista completa de detalles ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _clientServiceAPI.GetByIDServices(id);

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

        // GET: Crear cliente
        public IActionResult Create()
        {
            var model = new CreateClienteModel();
            return View(model);
        }

        // POST: Crear cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClienteModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var result = await _clientServiceAPI.SaveServicesPost(dto);

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
                    return View(dto);
                }

            }

            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // GET: Editar cliente
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _clientServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View(new UpdateClienteModel
                    {
                        Id = result.Data.Id,
                        Nombre = result.Data.Nombre,
                        Apellido = result.Data.Apellido,
                        Cedula = result.Data.Cedula,
                        Direccion = result.Data.Direccion,
                        Correo = result.Data.Correo,
                        Telefono = result.Data.Telefono
                    });
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

        // POST: Editar cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateClienteModel dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                var result = await _clientServiceAPI.UpdateServicesPut(dto);

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
                    return View(dto);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // --- Partial Delete ---
        public IActionResult _Delete(int id)
        {
            try
            {
                var result = _clientServiceAPI.GetByIDServices(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

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
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // POST: Confirm Delete
        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _clientServiceAPI.RemoveServicesPut(id);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    return PartialView("_DeleteSuccess", result.Message);
                }
                else
                {
                    return PartialView("_DeleteError", result.Message);
                }

            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, ErrorMessage = errorMessage });
            }
        }
    }
}
