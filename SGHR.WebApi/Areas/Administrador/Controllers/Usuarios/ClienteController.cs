using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Users.Cliente;
using SGHR.Application.Interfaces.Usuarios;

namespace SGHR.Web.Areas.Administrador.Controllers.Usuarios
{
    [Area("Administrador")]
    public class ClienteController : Controller
    {
        private readonly IClienteServices _clienteServices;

        public ClienteController(IClienteServices clienteServices)
        {
            _clienteServices = clienteServices;
        }


        // GET: ClienteController1
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // --- Partial para listar clientes ---
        public async Task<IActionResult> _List(string? cedula)
        {
            if (!string.IsNullOrEmpty(cedula))
            {
                var result = await _clienteServices.GetByCedulaAsync(cedula);
                if (!result.Success || result.Data == null)
                {
                    
                    return PartialView("_List", new List<ClienteDto>()); // lista vacía si no se encuentra
                }
                
                return PartialView("_List", new List<ClienteDto> { (ClienteDto)result.Data });
            }
            else
            {
                var result = await _clienteServices.GetAllAsync();
                if (!result.Success)
                {
                    
                    return PartialView("_Error", result.Message);
                }
                var listaClientes = result.Data as IEnumerable<ClienteDto>;
                return PartialView("_List", listaClientes);
            }
        }

        // GET: ClienteController1/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _clienteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            var cliente = result.Data as ClienteDto;
            return View(cliente); // Vista completa
        }

        // GET: ClienteController1/Create
        public IActionResult Create()
        {
            var model = new CreateClienteDto();
            return View(model); // Vista completa
        }

        // POST: ClienteController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClienteDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _clienteServices.CreateAsync(dto);
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

        // GET: ClienteController1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _clienteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View("_Error");
            }
            UpdateClienteDto cliente = new UpdateClienteDto
            {
                Id = result.Data.Id,
                Correo = result.Data.Correo,
                Nombre = result.Data.Nombre,
                Apellido = result.Data.Apellido,
                Cedula = result.Data.Cedula,
                Direccion = result.Data.Direccion,
                Telefono = result.Data.Telefono
            };
            return View(cliente); // Vista completa
        }

        // POST: ClienteController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateClienteDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _clienteServices.UpdateAsync(dto);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            TempData["Success"] = result.Message;
            return RedirectToAction("Index");
        }

        // GET: ClienteController1/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _clienteServices.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return PartialView("_Error");
            }
            if (result.Data == null)
            {
                TempData["Error"] = "Cliente no encontrado.";
                return PartialView("_Error");
            }
            return PartialView("_Delete", (ClienteDto)result.Data);

        }

        // POST: ClienteController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _clienteServices.DeleteAsync(id);
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
