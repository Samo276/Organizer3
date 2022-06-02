using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using Organizer3.Models.Enums;

namespace Organizer3.Controllers
{
    public class SiteFunctionsController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public SiteFunctionsController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> SiteFunctionsIndex()
        {
            if (await IsUserBlockedFromAccesingSiteFunctions())
                return RedirectToAction(nameof(Index), "Home");

            var tmp = _context.SiteFunctions.ToList();

            return View(
                tmp
            );
            
        }
        public async Task<IActionResult> ChangeSiteFunctionsStatus(int cId)
        {
            if (await IsUserBlockedFromAccesingSiteFunctions())
                return RedirectToAction(nameof(Index), "Home");

            var currentState = _context.SiteFunctions.First(y=>y.Id == cId).IsActive;
            _context.SiteFunctions.First(y => y.Id == cId).IsActive = !currentState;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SiteFunctionsIndex));
        }
        private async Task<bool> IsUserBlockedFromAccesingSiteFunctions()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.PartnerViewer != null)
                    if (tmp.PartnerViewer)
                        return false;
            }
            return true;
        }
    }
}
