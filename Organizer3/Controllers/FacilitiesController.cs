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
using Organizer3.Models.FacilitiesEditor;

namespace Organizer3.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public FacilitiesController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> FacilitiesIndex()
        {
            if(await IsUserBlockedFromAccesingFacilityEditor())
                return RedirectToAction(nameof(Index), "Home");

            return View(
                await ConvertDatabaseEntitiesToModel(
                    await _context.Facilities.ToListAsync()
                    )
                );
        }

        public async Task<IActionResult> Details(int? id)
        {
            
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Region,City,Adress")] FacilitiesListModel facilitiesListModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facilitiesListModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(FacilitiesIndex));
            }
            return View(facilitiesListModel);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FacilitiesListModel == null)
            {
                return NotFound();
            }

            var facilitiesListModel = await _context.FacilitiesListModel.FindAsync(id);
            if (facilitiesListModel == null)
            {
                return NotFound();
            }
            return View(facilitiesListModel);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Region,City,Adress")] FacilitiesListModel facilitiesListModel)
        {
            if (id != facilitiesListModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facilitiesListModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilitiesListModelExists(facilitiesListModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(FacilitiesIndex));
            }
            return View(facilitiesListModel);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FacilitiesListModel == null)
            {
                return NotFound();
            }

            var facilitiesListModel = await _context.FacilitiesListModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facilitiesListModel == null)
            {
                return NotFound();
            }

            return View(facilitiesListModel);
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FacilitiesListModel == null)
            {
                return Problem("Entity set 'OrganizerDbContext.FacilitiesListModel'  is null.");
            }
            var facilitiesListModel = await _context.FacilitiesListModel.FindAsync(id);
            if (facilitiesListModel != null)
            {
                _context.FacilitiesListModel.Remove(facilitiesListModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FacilitiesIndex));
        }

        private bool FacilitiesListModelExists(int id)
        {
          return (_context.FacilitiesListModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        /// <summary>
        /// Checks if user is logged in and is alowed to access the content of the Recruiter section, 
        /// returns FALSE if user is alowed in, and TRUE when access is forbidden
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsUserBlockedFromAccesingFacilityEditor()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.FacilityEditor != null)
                    if (tmp.FacilityEditor)
                        return false;
            }
            return true;
        }
        private async Task<List<FacilitiesListModel>> ConvertDatabaseEntitiesToModel(List<Facility> _data)
        {
            var tmp = new List<FacilitiesListModel>();
            foreach (var item in _data)
            {
                tmp.Add(new FacilitiesListModel {
                    Id = item.Id,
                    Name = item.Name,
                    Region = item.Region,
                    City = item.City,
                    Adress = item.Adress,
                    }
                );
            }
            return tmp;
        }
    }
}
