using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;

namespace Organizer3.Controllers
{
    public class ReadSingleAnnouncementController : Controller
    {
        private readonly OrganizerDbContext _context;

        public ReadSingleAnnouncementController(OrganizerDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ReadSingleAnnouncement(int id)
        {
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
    }
}
