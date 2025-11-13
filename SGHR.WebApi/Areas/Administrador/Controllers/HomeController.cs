using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGHR.Application.Dtos.Configuration.Operaciones.Pago;
using SGHR.Application.Interfaces.Operaciones;
using SGHR.Application.Interfaces.Reservas;
using System.Threading.Tasks;

namespace SGHR.Web.Areas.Administrador.Controllers
{
    [Area("Administrador")]
    public class HomeController : Controller
    {
        private readonly IPagoServices _pagoservices;
        private readonly IReservaServices _reservaServices;

        public HomeController(IPagoServices pagoServices, IReservaServices reservaServices)
        {
            _pagoservices = pagoServices;
            _reservaServices = reservaServices;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {
            var result = await _pagoservices.ObtenerResumenPagosAsync();
            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Login");
            }
            var resultPagos = await _pagoservices.ObtenerPagosAsync();
            if (!resultPagos.Success)
            {
                TempData["Error"] = resultPagos.Message;
                return RedirectToAction("Login");
            }
            var resultReservas = await _reservaServices.GetAllAsync();
            if (!resultReservas.Success)
            {
                TempData["Error"] = resultReservas.Message;
                return RedirectToAction("Login");
            }

            var DashboardViewModel = new Models.DashboardViewModel
            {
                Reservas = resultReservas.Data,
                Pagos = resultPagos.Data,
                Resumen = (ResumenPagoDto)result.Data

            };

            return View(DashboardViewModel);
        }

        // GET: HomeController/Details/5
        public ActionResult Privacy()
        {
            return View("Privacy", "Home");
        }
    }
}
