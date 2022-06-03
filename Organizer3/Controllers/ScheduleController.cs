using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.MyShop;

namespace Organizer3.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ScheduleController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult ScheduleIndex()
        {
            return View();
        }
        
        public async Task<IActionResult> AddShift()
        {
            return View();
        }
        public async Task<IActionResult> MoveShiftToArchive(int sId)
        {

                var tmp = _context.ShiftInfos.First(o => o.Id == sId);
                tmp.Archived=true;
                await _context.SaveChangesAsync();

                return RedirectToAction("MyShopIndex", "MyShop");

        }
        public async Task<IActionResult> SaveAddedShift(AddShiftModel fromView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var cuId = _userManager.GetUserId(User);
                    var cfId = _context.EmploymentStatuses.First(o => o.UserId == cuId).FacilityId;
                    var tmp = new ShiftInfo
                    {
                        ShiftName = fromView.Name,
                        FacilityId = (int)cfId,
                        StartingTime = new DateTime(1, 1, 1, fromView.StartingHour, 0, 0),
                        Duration = fromView.Duration,
                    };
                    _context.ShiftInfos.Add(tmp);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return RedirectToAction("AddShift");
                }
            }
            return RedirectToAction("MyShopIndex", "MyShop");

        }
    }
}
