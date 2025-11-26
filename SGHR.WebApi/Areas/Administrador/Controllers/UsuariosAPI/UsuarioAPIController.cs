using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.VisualBasic;
using SGHR.Web.Data;
using SGHR.Web.Models.Usuarios.Usuario;
using SGHR.Web.Services.Interfaces.Usuarios;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.UsuariosAPI
{
    [Area("Administrador")]
    public class UsuarioAPIController : Controller
    {
        private readonly IUsuarioServiceAPI _usuarioServiceAPI;

        public UsuarioAPIController(IUsuarioServiceAPI usuarioServiceAPI)
        {
            _usuarioServiceAPI = usuarioServiceAPI;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar usuarios ---
        public IActionResult _List(int? id)
        {
            try
            {
                if (id.HasValue && id > 0)
                {
                    var result = _usuarioServiceAPI.GetByIDServices((int)id);
                    if (result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return PartialView("_List", new List<UsuarioModel> { result.Data });
                    }
                    else
                    {
                        TempData["Error"] = result.Message;
                        return PartialView("_List", new List<UsuarioModel>());
                    }
                }
                else
                {
                    var usuarios = _usuarioServiceAPI.GetServices();
                    TempData["Success"] = "Lista cargada correctamente.";
                    return PartialView("_List", usuarios);
                }
            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }
            
        // --- Vista completa de detalles del usuario ---
        public IActionResult Details(int id)
        {
            try
            {
                var result = _usuarioServiceAPI.GetByIDServices(id);

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


            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, ErrorMessage = errorMessage });
            }
        }


        // GET: vista completa de creación de usuario
        public IActionResult Create()
        {
            var model = new CreateUsuarioModel();
            return View(model); // Vista completa, no partial
        }

        // POST: creación de usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUsuarioModel dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                var result = await _usuarioServiceAPI.SaveServicesPost(dto);

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);  
                if (!validate)
                {
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, errorMessage = errorMessage });
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
                

            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // GET: Editar usuario (vista completa)
        public IActionResult Edit(int id)
        {
            try
            {
                var result = _usuarioServiceAPI.GetByIDServices(id);


                if (result != null && result.Success)
                {

                    TempData["Success"] = result.Message;
                    return View(new UpdateUsuarioModel
                    {
                        Id = result.Data.Id,
                        Nombre = result.Data.Nombre,
                        Contraseña = result.Data.Contraseña,
                        Correo = result.Data.Correo,
                        Estado = result.Data.Estado,
                        Rol = result.Data.Rol
                    });
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Index");
                }
                

            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        // POST: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUsuarioModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _usuarioServiceAPI.UpdateServicesPut(model);

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
                    return View(model);
                }
                

            } catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }


        // --- Partial para eliminar ---
        public IActionResult _Delete(int id)
        {
            try
            {
                var result = _usuarioServiceAPI.GetByIDServices(id);

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


            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }

        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            try
            {
                var result = await _usuarioServiceAPI.RemoveServicesPut(id);

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


            }catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(500, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 500, errorMessage = errorMessage });
            }
        }
    }
}
