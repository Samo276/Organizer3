using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Data;
using Organizer3.Models;
using Organizer3.Models.EmployeeEditor;
using Organizer3.Models.EmployeeProfile;
using Organizer3.Models.Enums;

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

        public async Task<IActionResult> EmployeeProfileIndex(string employeeId, string? underprivelagedAccess)
        {
            if(underprivelagedAccess==null)
                if (await IsUserBlockedFromAccesingEmployeeProfile())
                    return RedirectToAction(nameof(Index), "Home");

            bool alowToEdit = false;
            if (underprivelagedAccess == null)
                alowToEdit = true;


            var tmpEmployee =  _context.Users.First(y => y.Id == employeeId);
            
            var photo = string.Empty;
            if(tmpEmployee.PhotoLocation != null) 
                photo = tmpEmployee.PhotoLocation;
            
            var tmpFacilityName = _context.Facilities.First(
                    y => y.Id == (_context.EmploymentStatuses.First(
                            u => u.UserId == employeeId).FacilityId
                            )
                    );
            
            var tmpLeaveList = _context.Leaves.Where(y=>y.UserId == employeeId).ToList();
        

            var toView = new EmployeeProfileDataModel(
                new UserDataEditModel(tmpEmployee),
                ConvertEmploymentStatusToModel(employeeId),
                GetUserAccessPrivilegesList(employeeId),
                photo,
                tmpFacilityName.Name + " " + tmpFacilityName.Adress + " " + tmpFacilityName.City,
                await ConvertLeavesDataToModel(tmpLeaveList),
                await CountLeaveDays(tmpLeaveList),
                alowToEdit
                ); 

            return View(toView);
        }
        public async Task<IActionResult> EmployeeProfileWorkplaceSelection(string id)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");

            var toView = new List<FacilitySelectModel>();            
            foreach(var item in _context.Facilities.ToList())
            {
                toView.Add(new FacilitySelectModel
                {
                    FacilityId = item.Id,
                    Name = item.Name + ": " + item.Region + " " + item.City + " " + item.PostalCode + " " + item.Adress,
                    UserId = id,
                });
            }

            return View(toView);
        }
        public async Task<IActionResult> ApplyWrokplaceChange(string uId, int fId)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");

            _context.EmploymentStatuses.First(y=>y.UserId== uId).FacilityId = fId;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EmployeeProfileIndex), new { employeeId = uId });
        }
        
        public async Task<IActionResult> AddNewLeave(string eId)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");

            var tmpUsername = _context.Users.First(y => y.Id == eId);
            var toView = new AddNewLeaveModel
            {
                UserId=eId,
                UserName = tmpUsername.LastName+" "+ tmpUsername.FirstName,
            };


            return View(toView);
        }
        public async Task<IActionResult> AddCreatedLeave(AddNewLeaveModel fromView)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");

            if (ModelState.IsValid)
            {
                var tmp = ConvertModelToLeaveDatabaseEntity(fromView);
                
                await _context.Leaves.AddAsync(tmp);
                await _context.SaveChangesAsync();

                return RedirectToAction("EmployeeProfileIndex", new { employeeId = fromView.UserId });
            }
            return RedirectToAction("AddNewLeave", new { employeeId = fromView.UserId });
        }
        public async Task<IActionResult>  DeleteLeaveEntity(int leaveId, string uId)
        {
            if (await IsUserBlockedFromAccesingEmployeeProfile())
                return RedirectToAction(nameof(Index), "Home");
            
            var tmp = _context.Leaves.First(y => y.Id == leaveId);
            
            _context.Leaves.Remove(tmp);
            await _context.SaveChangesAsync();
            return RedirectToAction("EmployeeProfileIndex", new { employeeId = tmp.UserId });
        }
        private Leave  ConvertModelToLeaveDatabaseEntity(AddNewLeaveModel fromView)
        {
            Enum.TryParse(fromView.LeaveType, out LeaveTypeEnum.LeaveTypeData value);
            var tmp = new Leave
            {
                UserId = fromView.UserId,
                LeaveType = value.ToString(),
                LeaveStart = fromView.LeaveStart,
                LeaveDuration = fromView.LeaveDuration,
                AuthorizerId = _userManager.GetUserId(User),
            };
            if (fromView.Note == null) 
                tmp.Note = String.Empty;
            else 
                tmp.Note = fromView.Note;

            return tmp;
        }

        private async Task<LeaveCounter> CountLeaveDays(List<Leave> tmpLeaveList)
        {
            var result = new LeaveCounter();
            foreach (var leave in tmpLeaveList.Where(y=>y.LeaveStart.Year==DateTime.Now.Year).ToList())
            {
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Wypoczynkowy.ToString())
                    result.Wypoczynkowy += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Okolicznościowy.ToString())
                    result.Okolicznosciowy += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Opieka.ToString())
                    result.Opieka += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Szkoleniowy.ToString())
                    result.Szkoleniowy += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Szkoleniowy.ToString())
                    result.Szkoleniowy += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Żądanie.ToString())
                    result.Zadanie += leave.LeaveDuration;
                if (leave.LeaveType == LeaveTypeEnum.LeaveTypeData.Bezpłatny.ToString())
                    result.Bezplatny += leave.LeaveDuration;
            }
            return result;
        }

        private async Task<List<LeaveDisplayModel>> ConvertLeavesDataToModel(List<Leave> tmpLeaveList)
        {
            var result = new List<LeaveDisplayModel>();
            if (tmpLeaveList.Any())
                foreach (var item in tmpLeaveList.OrderByDescending(y=>y.LeaveStart).ToList())
                {
                    var authorizer = await _context.Users.FirstAsync(y => y.Id == item.AuthorizerId);
                    result.Add(new LeaveDisplayModel
                    {
                        Id = item.Id,
                        LeaveType = item.LeaveType,
                        LeaveStart = item.LeaveStart,
                        LeaveEnd = item.LeaveStart.AddDays(item.LeaveDuration),
                        AuthorizerId = item.AuthorizerId,
                        AuthorizerName = authorizer.FirstName + " " + authorizer.LastName,
                        Note = item.Note,
                    });
                }
            return result;
        }

        public  List<string> GetUserAccessPrivilegesList(string employeeId)
        {
            var tmp = _context.AccessPermisions.First(y=>y.UserId==employeeId);
            var tmpList = new List<string>();
            if (tmp.Recruter) tmpList.Add("Rekrutacja");
            //if (tmp.Scheduler) tmpList.Add("Scheduler");
            //if (tmp.LeaveEditor) tmpList.Add("Urlopy");
            if (tmp.UserEditor) tmpList.Add("Edycja Użytkowników");
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
