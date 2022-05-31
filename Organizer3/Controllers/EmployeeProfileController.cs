using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using Organizer3.Models.EmployeeEditor;

namespace Organizer3.Controllers
{
    public class EmployeeProfileController : Controller
    {
        private readonly OrganizerDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public EmployeeProfileController(OrganizerDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        public async Task<IActionResult> EmployeeProfileIndex(string employeeId)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");

            var tmpEmployee =  _context.Users.First(y => y.Id == employeeId);
            
            var photo = string.Empty;
            if(tmpEmployee.PhotoLocation != null) 
                photo = tmpEmployee.PhotoLocation;
            
            var tmpFacilityName = _context.Facilities.First(
                    y => y.Id == (_context.EmploymentStatuses.First(
                            u => u.UserId == employeeId).FacilityId
                            )
                    );
            
            var toView = new EmployeeProfileDataModel(
                new UserDataEditModel(tmpEmployee),
                ConvertEmploymentStatusToModel(employeeId),
                GetUserAccessPrivilegesList(employeeId),
                photo,
                tmpFacilityName.Name+" "+tmpFacilityName.Adress+ " " + tmpFacilityName.City
                ); 

            return View(toView);
        }
        public  List<string> GetUserAccessPrivilegesList(string employeeId)
        {
            var tmp = _context.AccessPermisions.First(y=>y.UserId==employeeId);
            var tmpList = new List<string>();
            if (tmp.Recruter) tmpList.Add("Rekrutacja");
            //if (tmp.Scheduler) tmpList.Add("Scheduler");
            //if (tmp.LeaveEditor) tmpList.Add("Urlopy");
            if (tmp.UserEditor) tmpList.Add("UserEditor");
            if (tmp.UserViewer) tmpList.Add("Pracownicy");
            if (tmp.FacilityEditor) tmpList.Add("Sklepy");
            //if (tmp.FacilityViewer) tmpList.Add("");
            if (tmp.Announcer) tmpList.Add("Ogłoszenia");
            if (tmp.PersonalViewer) tmpList.Add("Mój Sklep");
            //if (tmp.PartnerViewer) tmpList.Add("");
            return tmpList;
        }
        public EmploymentStatusModel ConvertEmploymentStatusToModel(string id)
        {
            var getUserdata = _context.EmploymentStatuses.First(x => x.UserId == id);
            return new EmploymentStatusModel
            {
                Id = getUserdata.Id,
                UserId = getUserdata.UserId,
                IsEmployed = getUserdata.IsEmployed,
                Ocupation = getUserdata.Ocupation,
                EmployedSince = getUserdata.EmployedSince,
                ContractType = getUserdata.ContractType,
                ContractExpiration = getUserdata.ContractExpiration,
                SupervisorId = getUserdata.SupervisorId,
                otherInfo = getUserdata.otherInfo
            };
        }

        private async Task<bool> IsUserBlockedFromAccesingEmployeeProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var tmp = await _context.AccessPermisions.FirstAsync(u => u.UserId == _userManager.GetUserId(User));
                if (tmp.UserViewer != null)
                    if (tmp.UserViewer)
                        return false;
            }
            return true;
        }
    }
}
