using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Areas.Administrador.Models;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Services.Interfaces;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.DashBoardAPI
{
    [Area("Administrador")]
    public class HomeAPIController : Controller
    {
        private readonly IDashBoardServiceAPI _dashboardServiceAPI;
        public HomeAPIController(IDashBoardServiceAPI dashboardServiceAPI)
        {
            _dashboardServiceAPI = dashboardServiceAPI;
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {

            try
            {


                var result = await _dashboardServiceAPI.GetDashBoard();

                var validate = new ValidateStatusCode().ValidatorStatus(result.Statuscode, out string errorMessage);
                if (!validate && errorMessage != string.Empty)
                {
                    ViewBag.Error = errorMessage;
                    return RedirectToAction("ErrorPage", "Error", new { StatusCode = result.Statuscode, ErrorMessage = errorMessage });
                }

                if (result != null && result.Success)
                {
                    return View("Index", result.Data);
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                var validate = new ValidateStatusCode().ValidatorStatus(503, out string errorMessage);
                return RedirectToAction("ErrorPage", "Error", new { StatusCode = 503, ErrorMessage = errorMessage });
            }
        }

        // GET: HomeController/Details/5
        public ActionResult Privacy()
        {
            return View("Privacy", "HomeAPI");
        }
    }
}
