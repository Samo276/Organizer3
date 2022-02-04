using Microsoft.AspNetCore.Mvc;

namespace Organizer3.Controllers
{
    public class JobAplicationController : Controller
    {
        public IActionResult JobAplicationIndex()
        {
            return View();
        }
    }
}
