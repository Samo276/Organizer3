using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models.MyShop;
using Organizer3.Views.MyShop;
using Organizer3.Views.Schedule;

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
            tmp.Archived = true;
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
        public async Task<IActionResult> adhd(List<EmployeeNameAndIdModel> frv)
        {
            var masterdata = (string)ViewData["ReturnJson"];
            var tmp = JsonConvert.DeserializeObject<GenerateScheduleModel>(frv.First(o=>o.Id== "DataStorageForParsing").Name);

            foreach (var item in frv)
            {
                foreach (var item2 in tmp.ShiftWithAsignedEmployees.First(o => o.ShiftId == tmp.ShiftToEdit).EmployeesInShift.ToList())
                {
                    if(item2.Id==item.Id) item2.Participation = item.Participation;
                }
                
            }           

            return RedirectToAction("GenerateSchedule", new {fromView = JsonConvert.SerializeObject(tmp)});
        
        }

        public async Task<IActionResult> GenerateSchedule(string? fromView) //TODO -DZIALA kontynuoować z zapisywaniemz mian w bazie danych
        {
            if(!(fromView ==null || fromView == String.Empty)) { 
                var fromViewDeserialized = JsonConvert.DeserializeObject<GenerateScheduleModel>(fromView);
            
            if (fromViewDeserialized.AvailableShifts != null)
                return View(fromViewDeserialized);
            }
            var cuId = _userManager.GetUserId(User);
            var cfId = _context.EmploymentStatuses.First(o => o.UserId == cuId).FacilityId;

            var employees = _context.EmploymentStatuses
                .Where(o => o.FacilityId == cfId && o.IsEmployed == true)
                .Select(o => o.UserId)
                .ToList();

            var availableShifts = _context.ShiftInfos
                .Where(o => o.FacilityId == _context.EmploymentStatuses
                .First(o => o.UserId == cuId).FacilityId && o.Archived == false)
                .ToList();

            var toView = new GenerateScheduleModel
            {
                FromDay = DateTime.Now,
                TillDay = DateTime.Now,
                AvailableShifts = availableShifts,
                ShiftWithAsignedEmployees = GenerateBasic_EmployeesInShiftModel(GetUsernamesAndIds(employees), availableShifts),
            };

            return View(toView);
        }
        public async Task<IActionResult> AddHumanResources(string fromView, int shiftId)
        {
            var toView= JsonConvert.DeserializeObject<GenerateScheduleModel>(fromView);
            toView.ShiftToEdit = shiftId;
            var tmp = toView.ShiftWithAsignedEmployees.First(o=>o.ShiftId == shiftId).EmployeesInShift;
            tmp.Add(new EmployeeNameAndIdModel { Id = "DataStorageForParsing", Name = JsonConvert.SerializeObject(toView), Participation=false });
            return View(tmp);
        }
        private List<EmployeesInShiftModel> GenerateBasic_EmployeesInShiftModel(List<EmployeeNameAndIdModel> tmp, List<ShiftInfo>? availableShifts)
        {
            List<EmployeesInShiftModel> result = new();

            if (availableShifts != null)
                if (availableShifts.Any())
                    foreach (var tmpItem in availableShifts)
                    {
                        result.Add(new EmployeesInShiftModel
                        {
                            ShiftId = tmpItem.Id,
                            EmployeesInShift = tmp,
                        });
                    }

            return result;
        }

        private List<EmployeeNameAndIdModel> GetUsernamesAndIds(List<string> employees)
        {
            var result = new List<EmployeeNameAndIdModel>();
            var users = _context.Users.ToList();
            foreach (var item in employees)
            {
                var tmp = users.First(o => o.Id == item);
                result.Add(new EmployeeNameAndIdModel { Id = item, Name = tmp.LastName + " " + tmp.FirstName + "" });
            }
            return result;
        }

        
    }
}
