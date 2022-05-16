using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using System.Diagnostics;

namespace Organizer3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OrganizerDbContext  _context;
        private readonly UserManager<AppUser>  _userManager;



        public HomeController(OrganizerDbContext context, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated) // TODO - dodać blokadę dla pracowników niezatrudniuonych
            {
                var cuid = _userManager.GetUserId(User);
                var getPermissions = new UserAccess();
                if (_context.AccessPermisions.Where(x => x.UserId == cuid).Any())
                {
                    getPermissions = await _context.AccessPermisions.FirstAsync(x => x.UserId == cuid);
                }                
                var getNews = await _context.Announcements.OrderByDescending(x => x.CreationTime).Take(20).ToListAsync();
                var cu = new FunctionsListModel(getPermissions,getNews);

                return View(cu);
            }
            else
            {
                return View(new FunctionsListModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}