using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Areas.Administrador.Models;
using SGHR.Web.Data;
using SGHR.Web.Models;
using SGHR.Web.Models.Habitaciones.Amenity;
using SGHR.Web.Validador;

namespace SGHR.Web.Areas.Administrador.Controllers.DashBoardAPI
{
    [Area("Administrador")]
    public class HomeAPIController : Controller
    {
        public HomeAPIController()
        {
        }

        // GET: HomeController
        public async Task<IActionResult> Index()
        {

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:5020/api/");

                    var endpoint = await httpclient.GetAsync("DashBoard/GetDataDashBoard");

                    var validate = new ValidateStatusCode().ValidatorStatus((int)endpoint.StatusCode, out string errorMessage);
                    if (!validate && errorMessage != string.Empty)
                    {
                        ViewBag.Error = errorMessage;
                        return RedirectToAction("ErrorPage", "Error", new { StatusCode = (int)endpoint.StatusCode, ErrorMessage = errorMessage });
                    }

                    var result = await new JsonConvertidor<DashboardViewModel>().Deserializar(endpoint);

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

            }
            catch(Exception ex)
            {
                return View("Error", ex);
            }
        }

        // GET: HomeController/Details/5
        public ActionResult Privacy()
        {
            return View("Privacy", "HomeAPI");
        }
    }
}
