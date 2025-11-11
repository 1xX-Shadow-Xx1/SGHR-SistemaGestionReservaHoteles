using Humanizer;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Usuario;
using SGHR.Application.Interfaces.Usuarios;

namespace SGHR.Web.Controllers.Administrador.Usuarios
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
        }

        // Página principal
        public IActionResult Index()
        {
            return View();
        }

        // --- Partial para listar usuarios ---
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _usuarioServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<UsuarioDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<UsuarioDto> { (UsuarioDto)result.Data });
            }
            else
            {
                var result = await _usuarioServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaUsuarios = result.Data as IEnumerable<UsuarioDto>;
                return PartialView("_List", listaUsuarios);
            }
        }


        // --- Vista completa de detalles del usuario ---
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _usuarioServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var usuario = result.Data as UsuarioDto;
            return View(usuario); // Vista completa
        }


        // GET: vista completa de creación de usuario
        public IActionResult Create()
        {
            var model = new CreateUsuarioDto();
            return View(model); // Vista completa, no partial
        }

        // POST: creación de usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _usuarioServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista de usuarios o al detalle recién creado
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: Editar usuario (vista completa)
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _usuarioServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            } 
            UpdateUsuarioDto usuario = new UpdateUsuarioDto
            {
                Id = result.Data.Id,
                Correo = result.Data.Correo,
                Nombre = result.Data.Nombre,
                Contraseña = result.Data.Contraseña,
                Rol = result.Data.Rol,
                Estado = result.Data.Estado
            };
            return View(usuario); // Vista completa
        }

        // POST: Guardar cambios
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _usuarioServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }


        // --- Partial para eliminar ---
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _usuarioServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return PartialView("_Error");
            }
            return PartialView("_Delete", (UsuarioDto)result.Data);

        }

        [HttpPost, ActionName("_DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _usuarioServices.DeleteAsync(id);
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
