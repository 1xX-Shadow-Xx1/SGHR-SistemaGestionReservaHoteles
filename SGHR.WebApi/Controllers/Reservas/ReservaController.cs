using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Base;
using SGHR.Application.Dtos.Configuration.Reservas.Reserva;
using SGHR.Application.Interfaces.Reservas;

namespace SGHR.Web.Controllers.Reservas
{
    public class ReservaController : Controller
    {
        private readonly IReservaServices _reservaServices;

        public ReservaController(IReservaServices reservaServices)
        {
            _reservaServices = reservaServices;
        }

        // GET: ReservaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReservaController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ServiceResult result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
            {
                // Puedes redirigir a un error general o mostrar mensaje
                return RedirectToAction("Index");
            }

            var reserva = result.Data as ReservaDto;
            return View(reserva); // Vista completa
        }

        //GET: Partial para listar Habitaciones
        public async Task<IActionResult> _List(int? id)
        {
            if (id.HasValue && id > 0)
            {
                var result = await _reservaServices.GetByIdAsync(id.Value);
                if (!result.Success || result.Data == null)
                    return PartialView("_List", new List<ReservaDto>()); // lista vacía si no se encuentra

                return PartialView("_List", new List<ReservaDto> { (ReservaDto)result.Data });
            }
            else
            {
                var result = await _reservaServices.GetAllAsync();
                if (!result.Success)
                    return PartialView("_Error", result.Message);

                var listaReservas = result.Data as IEnumerable<ReservaDto>;
                return PartialView("_List", listaReservas);
            }
        }

        // GET: ReservaController/Create
        public IActionResult Create()
        {
            var model = new CreateReservaDto();
            return View(model); // Vista completa
        }

        // POST: ReservaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservaDto dto)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, devolver la misma vista con mensajes
                return View(dto);
            }

            var result = await _reservaServices.CreateAsync(dto);
            if (!result.Success)
            {
                // Si hay error en el servicio, mostrarlo en la vista
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            // Redirigir a la lista de habitaciones o al detalle recién creado
            return RedirectToAction("Index");
        }

        // GET: ReservaController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
                return View("_Error");  // o mostrar una página de error
            UpdateReservaDto habitacion = new UpdateReservaDto
            {
                Id = result.Data.Id,
                NumeroHabitacion = result.Data.NumeroHabitacion,
                CedulaCliente = result.Data.CedulaCliente,
                CorreoCliente = result.Data.CorreoCliente,
                FechaInicio = result.Data.FechaInicio,
                FechaFin = result.Data.FechaFin,
                Estado = result.Data.Estado
            };
            return View(habitacion); // Vista completa
        }

        // POST: ReservaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReservaDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var result = await _reservaServices.UpdateAsync(dto);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            // Redirigir a la lista después de guardar
            return RedirectToAction("Index");
        }

        // GET: ReservaController/Delete/5
        public async Task<IActionResult> _Delete(int id)
        {
            var result = await _reservaServices.GetByIdAsync(id);
            if (!result.Success)
                return PartialView("_Error");

            if (result.Data == null)
                return PartialView("_Error");

            return PartialView("_Delete", (ReservaDto)result.Data);

        }

        // POST: ReservaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _DeleteConfirmed(int id)
        {
            var result = await _reservaServices.DeleteAsync(id);
            if (!result.Success)
            {
                return Json(result);
            }
            return Json(result);
        }
    }
}
