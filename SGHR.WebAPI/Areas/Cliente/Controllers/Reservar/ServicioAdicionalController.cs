using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SGHR.Web.Areas.Cliente.Controllers.Reservar
{
    [Area("Cliente")]
    public class ServicioAdicionalController : Controller
    {
        // GET: ServicioAdicionalController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ServicioAdicionalController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ServicioAdicionalController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ServicioAdicionalController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: ServicioAdicionalController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ServicioAdicionalController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: ServicioAdicionalController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ServicioAdicionalController/Delete/5
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
