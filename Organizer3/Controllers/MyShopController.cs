using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.FacilitiesEditor;
using Organizer3.Models.MyShop;

namespace Organizer3.Controllers
{
    public class MyShopController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public MyShopController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> MyShopIndex()
        {
            if (await IsUserBlockedFromMyShop())
                return RedirectToAction(nameof(Index), "Home");

            try
            {
                var shopId = _context.EmploymentStatuses.First(y => y.UserId == _userManager.GetUserId(User));
                var getEmployeesIds = await _context.EmploymentStatuses.Where(y => y.FacilityId == shopId.FacilityId).ToListAsync();
                if (!getEmployeesIds.Any() || getEmployeesIds == null)
                    getEmployeesIds = new List<EmploymentStatus>();

                var schedule = new List<Atendance>();

                try
                {
                    foreach (var item in getEmployeesIds)
                    {
                        schedule.AddRange(_context.Atendances.Where(y => y.UserId == item.UserId).ToList());
                    }
                    schedule = schedule.Where(y => y.ShiftDate >= DateTime.Now).ToList();
                }
                catch
                {
                    schedule = new List<Atendance>();
                }

                var tmpShiftInfo = new List<ShiftInfo>();
                try
                {
                    tmpShiftInfo = await _context.ShiftInfos.Where(y => y.FacilityId == shopId.FacilityId && y.Archived == false).ToListAsync();
                }
                catch
                {
                    tmpShiftInfo = new List<ShiftInfo>();
                }

                

                
                var toView = new MyShopProfileModel
                {
                    FacilityData = await ConvertSingleFacilityDatabaseEntitiesToModel(_context.Facilities.First(y => y.Id == shopId.FacilityId)),
                    MyShopEmployeeModel = await _ConvertToMyShopEmployeeModel(getEmployeesIds),
                    MyShopShiftInfo= tmpShiftInfo,
                    MyShopSchedule = schedule,
                };
                return View(toView);
            }
            catch
            {
                return NotFound();
            }

        }

        private async Task<List<MyShopEmployeeModel>> _ConvertToMyShopEmployeeModel(List<EmploymentStatus> getEmployeesIds)
        {
            var result = new List<MyShopEmployeeModel>();
            foreach (var item in getEmployeesIds.Where(y=>y.IsEmployed==true).ToList())
            {
                result.Add(new MyShopEmployeeModel
                {
                    UserId = item.UserId,
                    Name = String.Empty,
                    PhoneNo = String.Empty,
                    Position = item.Ocupation,
                    PhotoLocation = String.Empty,
                });
            }
            var tmpUsers = await _context.Users.ToListAsync();

            foreach (var item in result)
            {
                var tmp = tmpUsers.First(y => y.Id == item.UserId);
                if (tmp.SecondaryName != null)
                    item.Name = tmp.LastName + " " + tmp.FirstName + " " + tmp.SecondaryName;
                else
                    item.Name = tmp.LastName + " " + tmp.FirstName;
                item.PhoneNo = tmp.PhoneNumber;
                item.PhotoLocation = tmp.PhotoLocation;
            }
            
            return result;
        }

        private async Task<bool> IsUserBlockedFromMyShop()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.FacilityViewer != null)
                    if (tmp.FacilityViewer)
                        return false;
            }
            return true;
        }
        private async Task<FacilitiesListModel> ConvertSingleFacilityDatabaseEntitiesToModel(Facility _data)
        {
            var result = await ConvertDatabaseEntitiesToModel(new List<Facility> { _data });
            return result.FirstOrDefault();
        }
        private async Task<List<FacilitiesListModel>> ConvertDatabaseEntitiesToModel(List<Facility> _data)
        {
            var tmp = new List<FacilitiesListModel>();
            foreach (var item in _data)
            {
                tmp.Add(new FacilitiesListModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Region = item.Region,
                    City = item.City,
                    Adress = item.Adress,
                    PostalCode = item.PostalCode,
                    PhoneNo = item.PhoneNo,
                    Info = item.AdditionalInfo,
                }
                );
            }
            return tmp;
        }
    }
}
