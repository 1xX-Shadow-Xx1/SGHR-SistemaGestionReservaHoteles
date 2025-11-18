using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGHR.Web.Areas.Administrador.Models;
using SGHR.Web.Models;

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

                    if (endpoint.IsSuccessStatusCode)
                    {
                        string response = await endpoint.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ServicesResultModel<DashboardViewModel>>(response);

                        if (result != null && result.Success)
                        {
                            return View("Index", result.Data);
                        }
                        else
                        {
                            return View("Error");
                        }
                    }
                    else
                    {
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
