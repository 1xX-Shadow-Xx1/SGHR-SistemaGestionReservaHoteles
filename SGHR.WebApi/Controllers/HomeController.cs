using Microsoft.AspNetCore.Mvc;

namespace SGHR.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeController/Details/5
        public ActionResult Privacy()
        {
            return View("Privacy", "Home");
        }
    }
}
