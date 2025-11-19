using Microsoft.AspNetCore.Mvc;
using SGHR.Web.Models.Error;

namespace SGHR.Web.Controllers
{
    public abstract class ErrorController : Controller
    {
        public virtual IActionResult ErrorPage(int statusCode, string message)
        {
            return View("Error", new ErrorViewModel
            {
                StatusCode = statusCode,
                ErrorMessage = message
            });
        }
    }
}
