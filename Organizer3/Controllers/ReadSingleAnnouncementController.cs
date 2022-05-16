using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;

namespace Organizer3.Controllers
{
    public class ReadSingleAnnouncementController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReadSingleAnnouncementController(OrganizerDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ReadSingleAnnouncement(int id)
        {
            if (await IsUserBlockedFromReadingSingleAnnouncment())
                return RedirectToAction(nameof(Index), "Home");
            try
            {
                var toView = await _context.Announcements.FirstAsync(a=>a.Id==id);
                return View(toView);
            }
            catch
            {
                return NotFound();
            }
            
        }
        /// <summary>
        /// Checks if user is logged in and is alowed to access the contents of Single Announcments section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromReadingSingleAnnouncment()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.EmploymentStatuses.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.IsEmployed)         
                        return false;
            }
            return true;
        }
    }
}
