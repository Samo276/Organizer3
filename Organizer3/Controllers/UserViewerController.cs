using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Organizer3.Controllers
{
    public class UserViewerController : Controller
    {
        // GET: UserViewerController
        public async Task<ActionResult> UserViewerIndex()
        {
            return View();
        }

        // GET: UserViewerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserViewerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserViewerController/Create
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

        // GET: UserViewerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserViewerController/Edit/5
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

        // GET: UserViewerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserViewerController/Delete/5
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
