#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;

namespace Organizer3.Controllers
{
    public class AnnouncementsController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AnnouncementsController(OrganizerDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Announcements
        public async Task<IActionResult> AnnouncerIndex()
        {
            if(await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index),"Home");

            return View(await _context.Announcements.OrderByDescending(x=>x.CreationTime).ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // GET: Announcements/Create
        public async Task<IActionResult> Create()
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,MessageContent")] AnnouncerCreateModel fromView)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var cu = await _context.Users.FirstAsync(x => x.Id == _userManager.GetUserId(User));
                var newAnn = new AnnouncerModel(
                    fromView.Id,
                    fromView.Title,
                    DateTime.Now,
                    cu.UserName + " " + cu.LastName,
                    fromView.MessageContent, "");
                _context.Add(newAnn);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AnnouncerIndex));
            }
            return View(fromView);
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }
            var toView = new AnnouncerCreateModel {
                Id=announcement.Id,
                Title=announcement.Title,
                MessageContent=announcement.MessageContent
                };
            return View(toView);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,MessageContent")] AnnouncerCreateModel fromView)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            if (id != fromView.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    var cu = await _context.Users.FirstAsync(x => x.Id == _userManager.GetUserId(User));
                    var legacyAnnouncement = await _context.Announcements.FirstAsync(a => a.Id == fromView.Id);
                    var announcement = await _context.Announcements.FirstAsync(x => x.Id == fromView.Id);
                    announcement.Title = fromView.Title;
                    announcement.MessageContent = fromView.MessageContent;
                    announcement.EditedBy = cu.UserName + " " + cu.LastName + " " + DateTime.Now; 
                    _context.Update(announcement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementExists(fromView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AnnouncerIndex));
            }
            return View(fromView);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await IsUserBlockedFromAccesingAnnouncer())
                return RedirectToAction(nameof(Index), "Home");

            var announcement = await _context.Announcements.FindAsync(id);
            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AnnouncerIndex));
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }
        /// <summary>
        /// Checks if user is logged in and is alowed to access the content of the Announcer section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromAccesingAnnouncer()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if(tmp.Announcer!=null)
                    if (tmp.Announcer)
                        return false;
            }
            return true;
        }
    }
}
