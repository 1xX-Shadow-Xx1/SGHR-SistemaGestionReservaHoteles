using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Interfaces.Usuarios;

namespace SGHR.Web.Controllers.Usuarios
{
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
            ServiceResult result = await _clienteServices.GetAllAsync();
            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View();
            }
            return View(result.Data);
        }

        // GET: ClienteController1/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        // GET: ClienteController1/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ClienteController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController1/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        // POST: ClienteController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController1/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

        // POST: ClienteController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
