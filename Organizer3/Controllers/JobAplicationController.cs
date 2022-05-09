using Microsoft.AspNetCore.Mvc;

namespace Organizer3.Controllers
{
    public class JobAplicationController : Controller
    {
        public async Task<IActionResult> JobAplicationIndex()
        {
            return View();
        }
        public async Task<IActionResult> SendJobAplication()
        {

            return View();
        }
    }
}
