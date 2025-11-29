using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Error;

namespace SGHR.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage(int statusCode, string errorMessage)
        {
            return View("ErrorPage", new ErrorViewModel
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage
            });
        }
    }
}
